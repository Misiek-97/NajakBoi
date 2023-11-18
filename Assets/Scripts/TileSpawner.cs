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
    public List<GameObject> tilesList = new List<GameObject>(); //Available Tiles
    public Dictionary<GameObject, Tile> tilesDictionary = new Dictionary<GameObject, Tile>(); //Available Tiles Dictionary
    
    private Dictionary<int, Tile> _gridTiles = new Dictionary<int, Tile>();
    
    public Vector2 tileSize;
    public Vector2 gridSize = new Vector2(5, 5);

    private void SetUpTileDictionary()
    {
        foreach (var t in tilesList)
        {
            var tile = t.GetComponent<Tile>();
            tilesDictionary.Add(t, tile);
        }
    }

    void Start()
    {
        SetUpTileDictionary();
        CreateGrid();
    }

    public void ClearGrid()
    {
        foreach (var t in _gridTiles)
        {
            Destroy(t.Value.gameObject);
        }
        _gridTiles.Clear();
    }
    
    
    public void LogGrid()
    {
        var sb = new StringBuilder();
        sb.Append("LOGGING GRID");
        foreach (var t in _gridTiles)
        {
            sb.AppendLine($"{t.Value.gameObject.name}");
        }
        Debug.Log(sb.ToString());
    }

    private TileType GetRandomTileType()
    {
        
        // Get all values from the enum
        Array enumValues = Enum.GetValues(typeof(TileType));

        // Choose a random index
        int randomIndex = Random.Range(0, enumValues.Length);

        // Return the corresponding enum value
        return (TileType)enumValues.GetValue(randomIndex);
    }

    private Tile SelectTile(TileType tileType, bool isRandom = false)
    {
        // Check if there are tile prefabs in the list
        if (tilesDictionary.Count == 0)
        {
            Debug.LogError("No tile prefabs specified in the list!");
            return null;
        }

        var type = tileType;
        if (isRandom)
            type = GetRandomTileType();
        
        var tile = tilesDictionary.Values.First(t => t.type == type);
        return tile ? tile : throw new InvalidOperationException($"Tile of Type {type.ToString()} is not in Tiles Dictionary!");
    }
    

    // Load a saved grid from a Dictionary
    public void LoadSavedGrid(Dictionary<Vector2, Tile> save)
    {
        if (tilesDictionary.Count == 0)
        {
            Debug.LogError("Tiles are not assigned!");
            return;
        }
        
        //Get Tile Size Vector2
        tileSize = tilePrefab.GetComponent<RectTransform>().rect.size;
        
        // Calculate the offset to center the grid
        float offsetX = (gridSize.x - 1) * tileSize.x / 2;
        float offsetY = (gridSize.y - 1) * tileSize.y / 2;
        
        ClearGrid();
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
            tile.position = new Vector2(x, y);
            tile.UpdateTile(SelectTile(t.Value.type));
            _gridTiles.Add(t.Value.id, tile);
        }
    }
    
    // Function to create a 2D grid of tiles
    public void CreateGrid(bool random = false)
    {
        if (tilesDictionary.Count == 0)
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

        ClearGrid();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Increment ID
                id++;
                
                // Calculate the position of each tile based on its index and tileSize
                Vector3 tilePosition = new Vector3(x * tileSize.x - offsetX, y * tileSize.y - offsetY, 0.0f);
                
                // Instantiate the tile prefab at the calculated position
                var tileGo = Instantiate(tilePrefab, transform);
                tileGo.transform.localPosition = tilePosition;
                tileGo.name = $"{tileGo.name}@X{x},Y{y}";
                
                var tile = tileGo.GetComponent<Tile>();
                tile.UpdateTile(SelectTile(TileType.Empty , random));
                tile.position = new Vector2(x, y);
                tile.id = id;
                _gridTiles.Add(id, tile);

            }
        }
    }
    

}
