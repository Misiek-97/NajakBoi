using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiles;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public List<GameObject> tilesList = new (); //Available Tiles
    private readonly List<Tile> _gridTiles = new (); //Active Tiles in the Grid
    private readonly Dictionary<GameObject, Tile> _tilesDictionary = new (); //Available Tiles Dictionary
    
    
    public Vector2 tileSize;
    public Vector2 gridSize = new (5, 5);

    private void SetUpTileDictionary()
    {
        foreach (var t in tilesList)
        {
            var tile = t.GetComponent<Tile>();
            _tilesDictionary.Add(t, tile);
        }
    }

    private void Start()
    {
        //Get Tile Size Vector2
        tileSize = tilePrefab.GetComponent<RectTransform>().rect.size;
        
        SetUpTileDictionary();
        var filePath = Application.persistentDataPath + "/TileGrid.json";

        if (File.Exists(filePath))
            LoadSavedGrid();
        else
        {
            CreateGrid();
        }
    }

    private void ClearGrid()
    {
        foreach (var t in _gridTiles)
        {
            Destroy(t.gameObject);
        }
        _gridTiles.Clear();
    }

    public void SaveGrid()
    {
        TileSerializer.SaveTileDataList(_gridTiles);
    }
    
    private static TileType GetRandomTileType()
    {
        
        // Get all values from the enum
        var enumValues = Enum.GetValues(typeof(TileType));

        // Choose a random index
        var randomIndex = Random.Range(0, enumValues.Length);

        // Return the corresponding enum value
        return (TileType)enumValues.GetValue(randomIndex);
    }

    private Tile SelectTile(TileType tileType, bool isRandom = false)
    {
        // Check if there are tile prefabs in the list
        if (_tilesDictionary.Count == 0)
        {
            Debug.LogError("No tile prefabs specified in the list!");
            return null;
        }

        var type = tileType;
        if (isRandom)
            type = GetRandomTileType();

        var tile = _tilesDictionary.Values.First(t => t.type == type);
        return tile ? tile : throw new InvalidOperationException($"Tile of Type {type.ToString()} is not in Tiles Dictionary!");
    }
    

    // Load grid from a save
    public void LoadSavedGrid()
    {
        ClearGrid();
        var tileDataList = TileSerializer.LoadTileDataList();

        var id = 0;
        foreach (var t in tileDataList)
        {
            InstantiateTileFromData(t, id);
            id++;
        }
    }
    
    // Function to create a 2D grid of tiles
    public void CreateGrid(bool random = false)
    {
        ClearGrid();
        
        var id = 0;
        
        for (var x = 0; x < gridSize.x; x++)
        {
            for (var y = 0; y < gridSize.y; y++)
            {
                id++;
                InstantiateTile(TileType.Empty, new Vector2(x, y), id, random);
            }
        }
    }

    private void InstantiateTile(TileType type, Vector2 gridPos, int id, bool random = false)
    {
        if (_tilesDictionary.Count == 0)
        {
            Debug.LogError("Tile prefabs are not assigned!");
            return;
        }

        // Calculate the offset to center the grid
        var offsetX = (gridSize.x - 1) * tileSize.x / 2;
        var offsetY = (gridSize.y - 1) * tileSize.y / 2;
        
        // Calculate the position of tile based on its index and tileSize and instantiate the tile prefab at the calculated position
        var tilePosition = new Vector3( gridPos.x * tileSize.x - offsetX, gridPos.y * tileSize.y - offsetY, 0.0f);
        var tileGo = Instantiate(tilePrefab, transform);
        tileGo.transform.localPosition = tilePosition;
                
        // Update Tile Parameters and add new tile to the list.
        var tile = tileGo.GetComponent<Tile>();
        var tileType = random ? GetRandomTileType() : type;
        tile.UpdateTile(SelectTile(tileType));
        tile.gridPos = gridPos;
        tile.id = id;
        tileGo.name = $"{tile.type}@({tile.gridPos.x},{tile.gridPos.y})#{tile.id}";
        _gridTiles.Add(tile);
    }
    private void InstantiateTileFromData(TileSerializer.TileData tileData, int id)
    {
        if (_tilesDictionary.Count == 0)
        {
            Debug.LogError("Tile prefabs are not assigned!");
            return;
        }
        
        // Calculate the offset to center the grid
        var offsetX = (gridSize.x - 1) * tileSize.x / 2;
        var offsetY = (gridSize.y - 1) * tileSize.y / 2;
        var pos = tileData.gridPos.ToVector2();
        
        // Calculate the position of tile based on its index and tileSize andInstantiate the tile prefab at the calculated position
        var tilePosition = new Vector3( pos.x * tileSize.x - offsetX, pos.y * tileSize.y - offsetY, 0.0f);
        var tileGo = Instantiate(tilePrefab, transform);
        tileGo.transform.localPosition = tilePosition;
                
        // Update Tile Parameters and add new tile to the list.
        var tileScript = tileGo.GetComponent<Tile>();
        tileScript.UpdateTile(SelectTile(tileData.tileType));
        tileScript.gridPos = pos;
        tileScript.id = id;
        tileGo.name = $"{tileScript.type}@({pos.x},{pos.y})#{id}";
        _gridTiles.Add(tileScript);
    }
    

}
