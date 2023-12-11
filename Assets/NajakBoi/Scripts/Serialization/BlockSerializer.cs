using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NajakBoi.Scripts.Blocks;
using UnityEngine;

namespace NajakBoi.Scripts.Serialization
{
    public class BlockSerializer : MonoBehaviour
    {
        // Your TileData class
        [Serializable]
        public class BlockData
        {
            public BlockType blockType;
            public SerializableVector2 gridPos;
            public bool isSpawn;

            // Constructor for creating a tile data
            public BlockData(BlockType type, SerializableVector2 position, bool spawn)
            {
                blockType = type;
                gridPos = position;
                isSpawn = spawn;
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
        public class BlockDataWrapper
        {
            public List<BlockData> blockDataList;

            public BlockDataWrapper(List<BlockData> list)
            {
                blockDataList = list;
            }
        }
        public static void SaveTileDataList(List<Block> blocks, PlayerId playerId)
        {
            var blockDataList = new List<string>();

            foreach (var block in blocks)
            {
                var blockData = new BlockData(block.type, new SerializableVector2(block.GridPos), block.isSpawn);

                // Serialize each BlockData individually
                blockDataList.Add(JsonUtility.ToJson(blockData));
            }

            // Wrap the list in a BlockDataWrapper
            var wrapper = new BlockDataWrapper(blockDataList.Select(JsonUtility.FromJson<BlockData>).ToList());

            // Specify the default file path for saving and loading
            var filePath = playerId switch
            {
                PlayerId.Player => Application.persistentDataPath + "/PlayerBlockGrid.json",
                PlayerId.Opponent => Application.persistentDataPath + "/OpponentBlockGrid.json",
                _ => ""
            };

            // Convert the wrapper to a JSON string
            var json = JsonUtility.ToJson(wrapper);
        
            File.WriteAllText(filePath, json);
        }

        public static List<BlockData> LoadBlockDataList(PlayerId playerId)
        {
            var filePath = playerId switch
            {
                PlayerId.Player => Application.persistentDataPath + "/PlayerBlockGrid.json",
                PlayerId.Opponent => Application.persistentDataPath + "/OpponentBlockGrid.json",
                _ => ""
            };

            if (File.Exists(filePath))
            {
                try
                {
                    // Read the JSON file as a single string
                    var json = File.ReadAllText(filePath);

                    // Deserialize the JSON string into the wrapper class
                    var wrapper = JsonUtility.FromJson<BlockDataWrapper>(json);

                    if (wrapper != null)
                    {
                        // Return the list from the wrapper
                        return wrapper.blockDataList;
                    }

                    Debug.LogError("Failed to deserialize BlockDataWrapper. Returning an empty list.");
                    return new List<BlockData>();

                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading BlockData from JSON: {e.Message}");
                    return new List<BlockData>();
                }
            }

            Debug.LogWarning($"{playerId}BlockGrid.json not found. Returning an empty list.");
            return new List<BlockData>();

        }


    }
}
