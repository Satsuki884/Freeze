using System.Collections.Generic;
using UnityEngine;

public enum DecorationType
{
    wall,
    floor
}

[CreateAssetMenu(fileName = "DecorationDatabase", menuName = "Game/Decoration Database")]
public class DecorationDatabase : ScriptableObject
{
    [System.Serializable]
    public class DecorationData
    {
        [SerializeField] private DecorationType _type;
        public DecorationType Type => _type;

        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField] private List<LocationType> _locationTypes;
        public List<LocationType> LocationTypes => _locationTypes;
    }

    public List<DecorationData> decorations = new();

    public GameObject GetRandomDecoration(LocationDatabase.LocationData location, DecorationType type)
    {
        List<DecorationData> pool = new();

        foreach (var decoration in decorations)
        {
            if (decoration.Type == type && decoration.LocationTypes.Contains(location.Type))
                pool.Add(decoration);
        }

        if (pool.Count == 0)
            return null;

        return pool[Random.Range(0, pool.Count)].Prefab;
    }
}