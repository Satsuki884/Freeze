using System.Collections.Generic;
using UnityEngine;

public class LocationScroller : MonoBehaviour
{
    public static LocationScroller Instance;

    void Awake()
    {
        Instance = this;
    }

    [Header("Database")]
    [SerializeField] private LocationDatabase locationDatabase;
    [SerializeField] private ObstacleDatabase obstacleDatabase;
    [SerializeField] private DecorationDatabase decorationDatabase;

    [Header("Settings")]
    [SerializeField] private int poolSize = 6;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Obstacle Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float obstacleSpawnChance = 0.35f;
    [SerializeField] private int maxEmptySegments = 3;
    [SerializeField] private int obstacleStartSegment = 4;

    [Header("Decoration Settings")]
    [SerializeField] private int minDecorations = 0;
    [SerializeField] private int maxDecorations = 4;

    private int emptySegmentsCounter = 0;
    private int spawnedSegments = 0;

    [Header("Score")]
    [SerializeField] private float scoreMultiplier = 1f;

    private float distanceTravelled = 0f;
    private int score = 0;

    private List<GameObject> activeSegments = new List<GameObject>();
    private List<float> segmentLengths = new List<float>();

    private bool isMoving = false;

    LocationDatabase.LocationData lastLocation;

    void Start()
    {
        GenerateInitialPool();
    }

    void Update()
    {
        if (!isMoving) return;

        MoveSegments();
        CheckRecycle();
        UpdateScore();
    }

    void UpdateScore()
    {
        distanceTravelled += moveSpeed * Time.deltaTime;

        int newScore = Mathf.FloorToInt(distanceTravelled * scoreMultiplier);

        if (newScore != score)
        {
            score = newScore;

            UIcontroller.Instance.SetScore(score);

            if (score > PlayerPrefs.GetInt("HighScore", 0))
                PlayerPrefs.SetInt("HighScore", score);
        }
    }

    void GenerateInitialPool()
    {
        float spawnX = 0;

        for (int i = 0; i < poolSize; i++)
        {
            var data = locationDatabase.GetRandomLocation(lastLocation);
            lastLocation = data;

            GameObject segment = Instantiate(
                data.Prefab,
                new Vector3(spawnX, 0, 0),
                Quaternion.identity,
                transform
            );

            activeSegments.Add(segment);
            segmentLengths.Add(data.Length);

            SpawnObstacle(segment);
            SpawnDecorations(segment, data);

            spawnX += data.Length;
        }
    }

    void MoveSegments()
    {
        foreach (var segment in activeSegments)
        {
            segment.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }

    void CheckRecycle()
    {
        GameObject first = activeSegments[0];
        float firstLength = segmentLengths[0];

        if (first.transform.position.x < -firstLength)
        {
            activeSegments.RemoveAt(0);
            segmentLengths.RemoveAt(0);

            Destroy(first);

            float newX = activeSegments[activeSegments.Count - 1].transform.position.x +
                         segmentLengths[segmentLengths.Count - 1];

            var data = locationDatabase.GetRandomLocation(lastLocation);
            lastLocation = data;

            GameObject newSegment = Instantiate(
                data.Prefab,
                new Vector3(newX, 0, 0),
                Quaternion.identity,
                transform
            );

            SpawnObstacle(newSegment);
            SpawnDecorations(newSegment, data);

            activeSegments.Add(newSegment);
            segmentLengths.Add(data.Length);
        }
    }

    void SpawnObstacle(GameObject segment)
    {
        spawnedSegments++;

        if (spawnedSegments < obstacleStartSegment)
            return;

        if (obstacleDatabase == null) return;

        bool spawn = Random.value < obstacleSpawnChance;

        if (emptySegmentsCounter >= maxEmptySegments)
            spawn = true;

        if (!spawn)
        {
            emptySegmentsCounter++;
            return;
        }

        GameObject obstaclePrefab = obstacleDatabase.GetRandomObstacle(lastLocation);

        if (obstaclePrefab == null)
        {
            emptySegmentsCounter++;
            return;
        }

        ObstaclePoint[] points = segment.GetComponentsInChildren<ObstaclePoint>();

        Vector3 spawnPosition;

        if (points.Length > 0)
        {
            ObstaclePoint randomPoint = points[Random.Range(0, points.Length)];
            spawnPosition = randomPoint.transform.position;
        }
        else
        {
            spawnPosition = segment.transform.position;
        }

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, segment.transform);

        emptySegmentsCounter = 0;
    }

    public void StartScrolling()
    {
        isMoving = true;
    }

    public void StopScrolling()
    {
        isMoving = false;
    }

    void SpawnDecorations(GameObject segment, LocationDatabase.LocationData location)
    {
        if (decorationDatabase == null) return;

        DecorPoint[] floorPoints = segment.GetComponentsInChildren<DecorPoint>();
        WallPoint[] wallPoints = segment.GetComponentsInChildren<WallPoint>();

        List<Transform> availableFloorPoints = new List<Transform>();
        List<Transform> availableWallPoints = new List<Transform>();

        foreach (var p in floorPoints)
            availableFloorPoints.Add(p.transform);

        foreach (var p in wallPoints)
            availableWallPoints.Add(p.transform);

        int count = Random.Range(minDecorations, maxDecorations + 1);

        for (int i = 0; i < count; i++)
        {
            bool spawnWall = Random.value > 0.5f;

            if (spawnWall && availableWallPoints.Count > 0)
            {
                int index = Random.Range(0, availableWallPoints.Count);
                Transform point = availableWallPoints[index];
                availableWallPoints.RemoveAt(index);

                GameObject prefab = decorationDatabase.GetRandomDecoration(location, DecorationType.wall);

                if (prefab != null)
                    Instantiate(prefab, point.position, Quaternion.identity, segment.transform);
            }
            else if (availableFloorPoints.Count > 0)
            {
                int index = Random.Range(0, availableFloorPoints.Count);
                Transform point = availableFloorPoints[index];
                availableFloorPoints.RemoveAt(index);

                GameObject prefab = decorationDatabase.GetRandomDecoration(location, DecorationType.floor);

                if (prefab != null)
                    Instantiate(prefab, point.position, Quaternion.identity, segment.transform);
            }
        }
    }
}