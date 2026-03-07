using System.Collections.Generic;
using UnityEngine;

public class LocationScroller : MonoBehaviour
{
    [Header("Database")]
    public LocationDatabase locationDatabase;
    public ObstacleDatabase obstacleDatabase;

    [Header("Settings")]
    public int poolSize = 6;
    public float moveSpeed = 5f;
    [Header("Score")]
    [SerializeField] private float scoreMultiplier = 1f;
    private float distanceTravelled = 0f;
    private int score = 0;

    private List<GameObject> activeSegments = new List<GameObject>();
    private List<float> segmentLengths = new List<float>();

    private bool isMoving = false;

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
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
    }

    void GenerateInitialPool()
    {
        float spawnX = 0;

        for (int i = 0; i < poolSize; i++)
        {
            SpawnSegment(spawnX);
            spawnX += segmentLengths[i];
        }
    }
    LocationDatabase.LocationData lastLocation;

    void SpawnSegment(float xPos)
    {
        // var data = locationDatabase.GetRandomLocation();
        var data = locationDatabase.GetRandomLocation(lastLocation);
        lastLocation = data;

        // Debug.Log(lastLocation.Name);

        GameObject segment = Instantiate(
            data.Prefab,
            new Vector3(xPos, 0, 0),
            Quaternion.identity,
            transform
        );

        // SpawnObstacle(segment);

        activeSegments.Add(segment);
        segmentLengths.Add(data.Length);
    }

    void SpawnObstacle(GameObject segment)
    {
        if (obstacleDatabase == null) return;

        if (Random.value < 0.5f) return;

        GameObject obstaclePrefab = obstacleDatabase.GetRandomObstacle(lastLocation);

        if (obstaclePrefab == null) return;

        Instantiate(obstaclePrefab, segment.transform);
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

            float newX = activeSegments[activeSegments.Count - 1].transform.position.x +
                         segmentLengths[segmentLengths.Count - 1];

            var data = locationDatabase.GetRandomLocation();

            first.transform.position = new Vector3(newX, 0, 0);

            SpawnObstacle(first);

            activeSegments.Add(first);
            segmentLengths.Add(data.Length);
        }
    }

    public void StartScrolling()
    {
        isMoving = true;
    }

    public void StopScrolling()
    {
        isMoving = false;
    }
}