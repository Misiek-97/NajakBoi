using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public int numberOfObjects = 5; // Specify the number of objects to instantiate
    public GameObject[] tilePrefabs; // The tile prefabs you want to instantiate

    void Start()
    {
        //Select random tile from array

        // Calculate the total width needed based on the RectTransform width of the prefab
        RectTransform prefabRectTransform = tilePrefabs[0].GetComponent<RectTransform>();
        float totalWidth = numberOfObjects * (prefabRectTransform.rect.width);

        // Calculate the starting position
        Vector3 startPosition = transform.position - new Vector3(totalWidth / 2f, 0f, 0f);

        // Instantiate objects next to each other
        for (int i = 0; i < numberOfObjects; i++)
        {
            var tile = SelectRandomTile();
            GameObject newObject = Instantiate(tile, startPosition + new Vector3(i * (prefabRectTransform.rect.width), 0f, 0f), Quaternion.identity, transform);
            newObject.transform.SetParent(transform); // Set the new object as a child of the current GameObject
        }
    }


    private GameObject SelectRandomTile()
    {
        // Check if there are tile prefabs in the list
        if (tilePrefabs.Length == 0)
        {
            Debug.LogError("No tile prefabs specified in the list!");
            return null;
        }

        // Randomly select a tile prefab from the list
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }
}
