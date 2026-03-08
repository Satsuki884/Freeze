using UnityEngine;

public class ObstaclePoint : MonoBehaviour
{
    [SerializeField] private ObstacleType _type;
    public ObstacleType Type => _type;
}