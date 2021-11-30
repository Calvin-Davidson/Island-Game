using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "World/PlaceableParser", order = 1)]
public class PlaceableParser : ScriptableObject
{
    [System.Serializable]
    public class PlaceableContainer
    {
        public GameObject SpawningObject;
        public Placeable Placeable;
    }

    [SerializeField] private List<PlaceableContainer> _placeableContainers = new List<PlaceableContainer>();


    public GameObject Parse(Placeable placeable)
    {
        return _placeableContainers.FirstOrDefault(container => container.Placeable == placeable).SpawningObject;
    }

    public Placeable Parse(GameObject placeableObject)
    {
        return _placeableContainers.FirstOrDefault(container => container.SpawningObject == placeableObject).Placeable;
    }
    
}
