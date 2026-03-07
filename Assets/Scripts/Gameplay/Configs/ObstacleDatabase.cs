using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleDatabase", menuName = "Game/Obstacle Database")]
public class ObstacleDatabase : ScriptableObject
{
    public List<GameObject> obstacles = new();

    public GameObject GetRandomObstacle()
    {
        if (obstacles.Count == 0) return null;

        return obstacles[Random.Range(0, obstacles.Count)];
    }
}