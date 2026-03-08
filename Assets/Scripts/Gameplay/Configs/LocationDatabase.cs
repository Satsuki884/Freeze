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

    Dictionary<string, LocationData> lookup;

    void OnEnable()
    {
        lookup = new Dictionary<string, LocationData>();

        foreach (var loc in locations)
        {
            if (string.IsNullOrWhiteSpace(loc.Name))
            {
                Debug.LogError("Location with empty name!");
                continue;
            }

            string key = loc.Name.Trim().ToLower();

            if (lookup.ContainsKey(key))
            {
                Debug.LogError("Duplicate location name: " + loc.Name);
                continue;
            }

            lookup.Add(key, loc);
        }
    }

    LocationData GetLocationByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        name = name.Trim().ToLower();

        if (lookup.TryGetValue(name, out var loc))
            return loc;

        Debug.LogError("Location not found: " + name);
        return null;
    }

    public LocationData GetRandomLocation(LocationData previous = null)
    {
        List<LocationData> pool = new();

        if (previous != null && previous.NextLocationNames != null && previous.NextLocationNames.Count > 0)
        {
            foreach (var name in previous.NextLocationNames)
            {
                var loc = GetLocationByName(name);

                if (loc != null)
                    pool.Add(loc);
            }

            // якщо список є, але жодної локації не знайдено
            if (pool.Count == 0)
            {
                Debug.LogError($"Location '{previous.Name}' has invalid NextLocationNames");
                return previous;
            }
        }
        else
        {
            // тільки якщо previous == null
            pool = locations;
        }

        int totalWeight = 0;

        foreach (var loc in pool)
            totalWeight += Mathf.Max(1, loc.Weight);

        int random = Random.Range(0, totalWeight);

        foreach (var loc in pool)
        {
            int weight = Mathf.Max(1, loc.Weight);

            if (random < weight)
                return loc;

            random -= weight;
        }

        return pool[0];
    }
}