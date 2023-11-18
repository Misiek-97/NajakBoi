using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public List<Tile> tiles = new List<Tile>(); //Available Tiles
    
    public Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();
    
    public Vector2 tileSize;
    public Vector2 gridSize = new Vector2(5, 5);
    
    void Start()
    {
        // Call the function to create the grid when the script starts
        CreateEmptyGrid();
    }

    public void ClearGrid()
    {
        foreach (var t in Tiles)
        {
            Destroy(t.Value.gameObject);
        }
        Tiles.Clear();
    }
    
    
    public void LogGrid()
    {
        var sb = new StringBuilder();
        sb.Append("LOGGING GRID");
        foreach (var t in Tiles)
        {
            sb.AppendLine($"{t.Value.gameObject.name}");
        }
        Debug.Log(sb.ToString());
    }


    private Tile SelectTile(TileType type, bool isRandom = false)
    {
        // Check if there are tile prefabs in the list
        if (tiles.Count == 0)
        {
            Debug.LogError("No tile prefabs specified in the list!");
            return null;
        }

        // Randomly select a tile prefab from the list
        if(isRandom)
            return tiles[Random.Range(0, tiles.Count)];

        var tile = tiles.First(t => t.type == type);
        if (!tile)
            throw new InvalidOperationException($"Could not find a tile of type {type.ToString()}");
        
        return tile;
    }
    

    // Load a saved grid from a Dictionary
    void LoadSavedGrid(Dictionary<Vector2, Tile> save)
    {
        if (tiles.Count == 0)
        {
            Debug.LogError("Tiles are not assigned!");
            return;
        }
        
        //Get Tile Size Vector2
        tileSize = tilePrefab.GetComponent<RectTransform>().rect.size;
        
        // Calculate the offset to center the grid
        float offsetX = (gridSize.x - 1) * tileSize.x / 2;
        float offsetY = (gridSize.y - 1) * tileSize.y / 2;

        foreach (var t in save)
        {
            var x = t.Key.x;
            var y = t.Key.y;
            // Calculate the position of each tile based on its index and tileSize
            Vector3 tilePosition = new Vector3(x * tileSize.x - offsetX, y * tileSize.y - offsetY, 0.0f);
                
            Debug.Log($"Creating tile X{x}, Y{y} at {tilePosition}");
            // Instantiate the tile prefab at the calculated position
            var tileGo = Instantiate(tilePrefab, transform);
            tileGo.transform.localPosition = tilePosition;
            tileGo.name = $"{tileGo.name}@X{x},Y{y}";
                
            // Update Tile Parameters
            
            var tile = tileGo.GetComponent<Tile>();
            tile.UpdateTile(SelectTile(t.Value.type));
            Tiles.Add(new Vector2(x, y), tile);
        }
        

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                
                // Calculate the position of each tile based on its index and tileSize
                Vector3 tilePosition = new Vector3(x * tileSize.x - offsetX, y * tileSize.y - offsetY, 0.0f);
                
                // Instantiate the tile prefab at the calculated position
                var tileGo = Instantiate(tilePrefab, transform);
                tileGo.transform.localPosition = tilePosition;
                tileGo.name = $"{tileGo.name}@X{x},Y{y}";
                
                // Update Tile Parameters
                var tile = tileGo.GetComponent<Tile>();
                Tiles.Add(new Vector2(x, y), tile);
            }
        }
    }
    
    // Function to create a 2D grid of tiles
    void CreateEmptyGrid()
    {
        if (tiles.Count == 0)
        {
            Debug.LogError("Tile prefabs are not assigned!");
            return;
        }
        
        //Get Tile Size Vector2
        tileSize = tilePrefab.GetComponent<RectTransform>().rect.size;
        
        // Calculate the offset to center the grid
        var offsetX = (gridSize.x - 1) * tileSize.x / 2;
        var offsetY = (gridSize.y - 1) * tileSize.y / 2;
        var id = 0;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Increment ID
                id++;
                
                // Calculate the position of each tile based on its index and tileSize
                Vector3 tilePosition = new Vector3(x * tileSize.x - offsetX, y * tileSize.y - offsetY, 0.0f);
                
                // Instantiate the tile prefab at the calculated position
                GameObject tile = Instantiate(tilePrefab, transform);

                // Set up Empty Tile
                tile.transform.localPosition = tilePosition;
                tile.name = $"{tile.name}@X{x},Y{y}";
                var tileScript = tile.GetComponent<Tile>();
                tileScript.id = id;
                Tiles.Add(new Vector2(x, y), tileScript);
            }
        }
    }
    

}
