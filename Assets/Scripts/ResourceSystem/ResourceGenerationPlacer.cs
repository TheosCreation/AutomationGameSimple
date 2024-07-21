using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerationPlacer : MonoBehaviour
{
    //[SerializeField] private GroundLayer groundLayer;
    [SerializeField] private ResourceLayer resourceLayer;
    [SerializeField] private List<Resource> resourcesToPlace;
    [SerializeField] private int numberOfGroups = 10;
    [SerializeField] private int minGroupSize = 3;
    [SerializeField] private int maxGroupSize = 10;
    [SerializeField] private Vector2 mapSize = new Vector2(50, 50);

    private void Start()
    {
        GenerateResourceGroups();
    }

    private void GenerateResourceGroups()
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            int groupSize = Random.Range(minGroupSize, maxGroupSize);
            PlaceResourceGroup(randomPosition, groupSize);
        }
    }

    private Vector2 GetRandomPosition()
    {
        float x = Random.Range(-mapSize.x, mapSize.x);
        float y = Random.Range(-mapSize.y, mapSize.y);
        return new Vector2(x, y);
    }

    private Resource GetRandomResource()
    {
        return resourcesToPlace[Random.Range(0, resourcesToPlace.Count)];
    }

    private void PlaceResourceGroup(Vector2 startPosition, int groupSize)
    {
        for (int i = 0; i < groupSize; i++)
        {
            Vector2 resourcePosition;
            do
            {
                Vector2 offset = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
                resourcePosition = startPosition + offset;
            } while (!resourceLayer.IsEmpty(resourcePosition));

            Resource resourceToPlace = GetRandomResource();
            resourceLayer.PlaceResource(resourcePosition, resourceToPlace, Vector2.right);
        }
    }
}
