using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blocks
{
    public class BlockSpawner : MonoBehaviour
    {
        public GameObject blockPrefab;
        public List<GameObject> blocksList = new (); //Available Tiles
        private readonly List<Block> _gridBlocks = new (); //Active Tiles in the Grid
        private readonly Dictionary<GameObject, Block> _blocksDictionary = new (); //Available Tiles Dictionary
    
    
        public Vector2 blockSize;
        public Vector2 gridSize = new (5, 5);

        private void SetUpBlockDictionary()
        {
            foreach (var b in blocksList)
            {
                var block = b.GetComponent<Block>();
                _blocksDictionary.Add(b, block);
            }
        }

        private void Start()
        {
            SetUpBlockDictionary();
            var filePath = Application.persistentDataPath + "/BlockGrid.json";

            if (File.Exists(filePath))
            {
                LoadSavedGrid();
            }
            else
            {
                CreateGrid();
            }
        }

        private void ClearGrid()
        {
            foreach (var t in _gridBlocks)
            {
                Destroy(t.gameObject);
            }
            _gridBlocks.Clear();
        }

        public void SaveGrid()
        {
            BlockSerializer.SaveTileDataList(_gridBlocks);
        }
    
        private static BlockType GetRandomBlockType()
        {
        
            // Get all values from the enum
            var enumValues = Enum.GetValues(typeof(BlockType));

            // Choose a random index
            var randomIndex = Random.Range(0, enumValues.Length);

            // Return the corresponding enum value
            return (BlockType)enumValues.GetValue(randomIndex);
        }

        private Block SelectBlock(BlockType blockType, bool isRandom = false)
        {
            // Check if there are tile prefabs in the list
            if (_blocksDictionary.Count == 0)
            {
                Debug.LogError("No tile prefabs specified in the list!");
                return null;
            }

            var type = blockType;
            if (isRandom)
                type = GetRandomBlockType();

            var block = _blocksDictionary.Values.First(b => b.type == type);
            return block ? block : throw new InvalidOperationException($"Tile of Type {type.ToString()} is not in Tiles Dictionary!");
        }
    

        // Load grid from a save
        public void LoadSavedGrid()
        {
            ClearGrid();
            var blockDataList = BlockSerializer.LoadBlockDataList();

            var id = 0;
            foreach (var b in blockDataList)
            {
                InstantiateBlockFromData(b, id);
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
                    InstantiateBlock(BlockType.Empty, new Vector2(x, y), id, random);
                }
            }
        }

        private void InstantiateBlock(BlockType type, Vector2 gridPos, int id, bool random = false)
        {
            if (_blocksDictionary.Count == 0)
            {
                Debug.LogError("Block prefabs are not assigned!");
                return;
            }

            // Calculate the offset to center the grid
            var offsetX = (gridSize.x - 1) * blockSize.x / 2;
            var offsetY = (gridSize.y - 1) * blockSize.y / 2;
        
            // Calculate the position of tile based on its index and tileSize and instantiate the tile prefab at the calculated position
            var blockPosition = new Vector3( gridPos.x * blockSize.x - offsetX, gridPos.y * blockSize.y - offsetY, 0.0f);
            var blockGo = Instantiate(blockPrefab, transform);
            blockGo.transform.localPosition = blockPosition;
                
            // Update Tile Parameters and add new tile to the list.
            var block = blockGo.GetComponent<Block>();
            var blockType = random ? GetRandomBlockType() : type;
            block.UpdateBlockProperties(SelectBlock(blockType));
            block.gridPos = gridPos;
            block.id = id;
            blockGo.name = $"{block.type}@({block.gridPos.x},{block.gridPos.y})#{block.id}";
            _gridBlocks.Add(block);
        }
        private void InstantiateBlockFromData(BlockSerializer.BlockData blockData, int id)
        {
            if (_blocksDictionary.Count == 0)
            {
                Debug.LogError("Tile prefabs are not assigned!");
                return;
            }
        
            // Calculate the offset to center the grid
            var offsetX = (gridSize.x - 1) * blockSize.x / 2;
            var offsetY = (gridSize.y - 1) * blockSize.y / 2;
            var pos = blockData.gridPos.ToVector2();
        
            // Calculate the position of block based on its index and tileSize and instantiate the block prefab at the calculated position
            var blockPosition = new Vector3( pos.x * blockSize.x - offsetX, pos.y * blockSize.y - offsetY, 0.0f);
            var blockGo = Instantiate(blockPrefab, transform);
            blockGo.transform.localPosition = blockPosition;
                
            // Update Tile Parameters and add new tile to the list.
            var blockScript = blockGo.GetComponent<Block>();
            blockScript.UpdateBlockProperties(SelectBlock(blockData.blockType));
            blockScript.gridPos = pos;
            blockScript.id = id;
            blockGo.name = $"{blockScript.type}@({pos.x},{pos.y})#{id}";
            _gridBlocks.Add(blockScript);
        }
    

    }
}
