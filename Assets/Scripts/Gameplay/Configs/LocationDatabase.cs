using System.Collections.Generic;
using UnityEngine;

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
    }

    public List<LocationData> locations = new();

    public LocationData GetRandomLocation()
    {
        int totalWeight = 0;

        foreach (var loc in locations)
            totalWeight += loc.Weight;

        int random = Random.Range(0, totalWeight);

        foreach (var loc in locations)
        {
            if (random < loc.Weight)
                return loc;

            random -= loc.Weight;
        }

        return locations[0];
    }
}