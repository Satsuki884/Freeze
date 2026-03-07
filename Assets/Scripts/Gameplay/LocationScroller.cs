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
    }

    void GenerateInitialPool()
    {
        float spawnZ = 0;

        for (int i = 0; i < poolSize; i++)
        {
            SpawnSegment(spawnZ);
            spawnZ += segmentLengths[i];
        }
    }

    void SpawnSegment(float zPos)
    {
        var data = locationDatabase.GetRandomLocation();

        GameObject segment = Instantiate(data.Prefab, new Vector3(0, 0, zPos), Quaternion.identity, transform);

        SpawnObstacle(segment);

        activeSegments.Add(segment);
        segmentLengths.Add(data.Length);
    }

    void SpawnObstacle(GameObject segment)
    {
        if (obstacleDatabase == null) return;

        if (Random.value < 0.5f) return;

        GameObject obstaclePrefab = obstacleDatabase.GetRandomObstacle();

        if (obstaclePrefab == null) return;

        // Instantiate(obstaclePrefab, segment.transform);
        Transform point = segment.transform.Find("ObstaclePoint");

        if (point != null)
            Instantiate(obstaclePrefab, point.position, Quaternion.identity, segment.transform);
    }

    void MoveSegments()
    {
        foreach (var segment in activeSegments)
        {
            segment.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }

    void CheckRecycle()
    {
        GameObject first = activeSegments[0];
        float firstLength = segmentLengths[0];

        if (first.transform.position.z < -firstLength)
        {
            activeSegments.RemoveAt(0);
            segmentLengths.RemoveAt(0);

            float newZ = activeSegments[activeSegments.Count - 1].transform.position.z +
                         segmentLengths[segmentLengths.Count - 1];

            var data = locationDatabase.GetRandomLocation();

            first.transform.position = new Vector3(0, 0, newZ);

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