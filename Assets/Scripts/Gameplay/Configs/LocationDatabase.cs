using System.Collections.Generic;
using UnityEngine;

public enum LocationType
{
    library,
    museum,
    galery,
    storage,
    connector
}

[CreateAssetMenu(fileName = "LocationDatabase", menuName = "Game/Location Database")]
public class LocationDatabase : ScriptableObject
{
    [System.Serializable]
    public class LocationData
    {
        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField] private float _length = 20f;
        public float Length => _length;

        [SerializeField] private int _weight = 1;
        public int Weight => _weight;

        [SerializeField] private List<string> _nextLocationNames;
        public List<string> NextLocationNames => _nextLocationNames;

        [SerializeField] private LocationType _type;
        public LocationType Type => _type;
    }

    public List<LocationData> locations = new();

    LocationData GetLocationByName(string name)
    {
        foreach (var loc in locations)
        {
            if (loc.Name == name)
                return loc;
        }

        return null;
    }

    public LocationData GetRandomLocation(LocationData previous = null)
    {
        List<LocationData> pool = new List<LocationData>();

        if (previous != null && previous.NextLocationNames != null && previous.NextLocationNames.Count > 0)
        {
            foreach (var name in previous.NextLocationNames)
            {
                var loc = GetLocationByName(name);
                if (loc != null)
                    pool.Add(loc);
            }
        }

        if (pool.Count == 0)
            pool = locations;

        int totalWeight = 0;

        foreach (var loc in pool)
            totalWeight += loc.Weight;

        int random = Random.Range(0, totalWeight);

        foreach (var loc in pool)
        {
            if (random < loc.Weight)
                return loc;

            random -= loc.Weight;
        }

        return pool[0];
    }
}