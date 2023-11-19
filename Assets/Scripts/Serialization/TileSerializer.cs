using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tiles;

public class TileSerializer : MonoBehaviour
{
    // Your TileData class
    [Serializable]
    public class TileData
    {
        public TileType tileType;
        public SerializableVector2 gridPos;

        // Constructor for creating a tile data
        public TileData(TileType type, SerializableVector2 position)
        {
            tileType = type;
            gridPos = position;
        }
    }
    
    [Serializable]
    public class SerializableVector2
    {
        public float x;
        public float y;

        public SerializableVector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
    
    [Serializable]
    public class TileDataWrapper
    {
        public List<TileData> tileDataList;

        public TileDataWrapper(List<TileData> list)
        {
            tileDataList = list;
        }
    }
    public static void SaveTileDataList(List<Tile> tiles)
    {
        var serializedTileDataList = new List<string>();

        foreach (var tile in tiles)
        {
            var tileData = new TileData(tile.type, new SerializableVector2(tile.gridPos));

            // Serialize each TileData individually
            serializedTileDataList.Add(JsonUtility.ToJson(tileData));
        }

        // Wrap the list in a TileDataWrapper
        var wrapper = new TileDataWrapper(serializedTileDataList.Select(JsonUtility.FromJson<TileData>).ToList());

        // Specify the default file path for saving and loading
        var filePath = Application.persistentDataPath + "/TileGrid.json";

        // Convert the wrapper to a JSON string
        var json = JsonUtility.ToJson(wrapper);
        
        File.WriteAllText(filePath, json);
    }

    public static List<TileData> LoadTileDataList()
    {
        var filePath = Application.persistentDataPath + "/TileGrid.json";

        if (File.Exists(filePath))
        {
            try
            {
                // Read the JSON file as a single string
                var json = File.ReadAllText(filePath);

                // Deserialize the JSON string into the wrapper class
                var wrapper = JsonUtility.FromJson<TileDataWrapper>(json);

                if (wrapper != null)
                {
                    // Return the list from the wrapper
                    return wrapper.tileDataList;
                }

                Debug.LogError("Failed to deserialize TileDataWrapper. Returning an empty list.");
                return new List<TileData>();

            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading TileData from JSON: {e.Message}");
                return new List<TileData>();
            }
        }

        Debug.LogWarning("TileGrid.json not found. Returning an empty list.");
        return new List<TileData>();

    }


}
