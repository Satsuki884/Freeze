using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    jump,
    slide
}

[CreateAssetMenu(fileName = "ObstacleDatabase", menuName = "Game/Obstacle Database")]
public class ObstacleDatabase : ScriptableObject
{
    [System.Serializable]
    public class ObstacleData
    {
        [SerializeField] private ObstacleType _type;
        public ObstacleType Type => _type;

        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField] private LocationType _locationType;
        public LocationType LocationType => _locationType;
    }

    public List<ObstacleData> obstacles = new();

    public GameObject GetRandomObstacle(LocationDatabase.LocationData location)
    {
        List<ObstacleData> pool = new();

        foreach (var obstacle in obstacles)
        {
            if (obstacle.LocationType == location.Type)
                pool.Add(obstacle);
        }

        if (pool.Count == 0)
            return null;

        return pool[Random.Range(0, pool.Count)].Prefab;
    }
}