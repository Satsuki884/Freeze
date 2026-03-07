using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationDatabase", menuName = "Game/Location Database")]
public class LocationDatabase : ScriptableObject
{
    [System.Serializable]
    public class LocationData
    {
        [SerializeField] private string _locationName;
        public string LocationName => _locationName;
        [SerializeField] private GameObject _locationPrefab;
        public GameObject LocationPrefab => _locationPrefab;
    }

    [Header("Locations")]
    [SerializeField] private List<LocationData> locations = new List<LocationData>();

    public GameObject GetRandomLocation()
    {
        if (locations.Count == 0) return null;

        int index = Random.Range(0, locations.Count);
        return locations[index].LocationPrefab;
    }

    public GameObject GetLocationByName(string name)
    {
        foreach (var loc in locations)
        {
            if (loc.LocationName == name)
                return loc.LocationPrefab;
        }

        return null;
    }
}