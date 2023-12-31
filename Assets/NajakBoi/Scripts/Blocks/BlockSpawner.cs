using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NajakBoi.Scripts.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using NajakBoi.Scripts.Session;

namespace NajakBoi.Scripts.Blocks
{
    public class BlockSpawner : MonoBehaviour
    {
        public GameObject blockPrefab;
        public List<GameObject> blocksList = new (); //Available Tiles
        public bool isPlayer;

        public PlayerId playerId;
        
        public readonly List<Block> GridBlocks = new (); //Active Tiles in the Grid
        private readonly Dictionary<GameObject, Block> _blocksDictionary = new (); //Available Tiles Dictionary
        
        public Vector2 blockSize;
        public Vector2 gridSize = new (5, 5);

        private bool SetUpBlockDictionary()
        {
            foreach (var b in blocksList)
            {
                var block = b.GetComponent<Block>();
                _blocksDictionary.Add(b, block);
            }

            return true;
        }

        public void RefreshAllBlocks()
        {
            foreach (var b in GridBlocks)
            {
                b.Renderer.enabled = b.type != BlockType.Empty;
            }
        }

        private async void Start()
        {
            while (!SetUpBlockDictionary())
                await Task.Delay(10);

            if (!isPlayer)
            {
                playerId = PlayerId.Opponent;
                if (GameManager.GameMode == GameMode.Expedition)
                {
                    CreateGrid(true);
                }
                else
                {
                    var filePath = Application.persistentDataPath + "/OpponentBlockGrid.json";
                    if (File.Exists(filePath))
                    {
                        LoadSavedGrid();
                    }
                    else
                    {
                        CreateGrid();
                    }
                }
            }
            else
            {
                playerId = PlayerId.Player;
                
                var filePath = Application.persistentDataPath + "/PlayerBlockGrid.json";
                CreateGrid();
                if (File.Exists(filePath))
                {
                    LoadSavedGrid();
                }
                
                if (GameManager.GameMode == GameMode.Expedition)
                    GameManager.Instance.StartGame();
            }
            
            
        }

        private void ClearGrid()
        {
            foreach (var t in GridBlocks)
            {
                Destroy(t.gameObject);
            }
            GridBlocks.Clear();
        }

        public void SaveGrid()
        {
            BlockSerializer.SaveTileDataList(GridBlocks, playerId);
        }
    
        private static BlockType GetRandomBlockType(List<BlockType> excludedTypes = null)
        {
        
            // Get all values from the enum
            var enumValues = Enum.GetValues(typeof(BlockType));
            var randomIndex = 0;
            
            if (excludedTypes != null)
            {
                // Convert enum values to a list and exclude the specified type
                var validValues = enumValues.Cast<BlockType>().Except(excludedTypes).ToList();
                // Choose a random index from the filtered list
                randomIndex = Random.Range(0, validValues.Count);

                // Return the corresponding enum value
                return validValues[randomIndex];
            }

            // Choose a random index
            randomIndex = Random.Range(0, enumValues.Length);

            // Return the corresponding enum value
            return (BlockType)enumValues.GetValue(randomIndex);
        }

        private Block SelectBlock(BlockType blockType, bool isRandom = false, List<BlockType> excludedTypes = null)
        {
            // Check if there are tile prefabs in the list
            if (_blocksDictionary.Count == 0)
            {
                Debug.LogError("No tile prefabs specified in the list!");
                return null;
            }

            var type = blockType;
            
            if (isRandom)
                type = GetRandomBlockType(excludedTypes);
            
            if (excludedTypes is { Count: > 0 } && excludedTypes.Contains(blockType))
                type = BlockType.Empty;

            var block = _blocksDictionary.Values.First(b => b.type == type);
            return block ? block : throw new InvalidOperationException($"Tile of Type {type.ToString()} is not in Tiles Dictionary!");
        }
    

        // Load grid from a save
        public void LoadSavedGrid()
        {
            var blockDataList = BlockSerializer.LoadBlockDataList(playerId);

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

            if (playerId == PlayerId.Player)
            {
                gridSize.x = SessionManager.PlayerData.BuildingStats.maxBuildX;
                gridSize.y = SessionManager.PlayerData.BuildingStats.maxBuildY;
            }
            
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
                
            // Set Up Exclusions
            var excludedTypes = new List<BlockType>();
            if (gridPos.y == 0)
            {
                excludedTypes.Add(BlockType.MilitaryChest);
            }
            else
            {
                var blockBelow = GridBlocks.Find(x => x.GridPos == new Vector2(gridPos.x, gridPos.y - 1));
                if(!blockBelow || blockBelow.type == BlockType.MilitaryChest || blockBelow.type == BlockType.Empty )
                    excludedTypes.Add(BlockType.MilitaryChest);
            }

            if (playerId == PlayerId.Player || (playerId == PlayerId.Opponent && GameManager.GameMode != GameMode.Expedition))
                excludedTypes.Add(BlockType.MilitaryChest);

            // Update Tile Parameters and add new tile to the list.
            var block = blockGo.GetComponent<Block>();
            var blockType = random ? GetRandomBlockType(excludedTypes: excludedTypes) : type;
            
            if (random && playerId == PlayerId.Player)
            {
                if (GetTotalWeight() + SelectBlock(blockType).weight >
                    SessionManager.PlayerData.BuildingStats.maxWeight)
                    blockType = BlockType.Empty;
            }
            
            block.UpdateBlockProperties(SelectBlock(blockType, excludedTypes: excludedTypes), false);
            block.GridPos = gridPos;
            block.ID = id;
            blockGo.name = $"{block.type}@({block.GridPos.x},{block.GridPos.y})#{block.ID}";
            GridBlocks.Add(block);
            
            
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
            
            // Remove existing block
            var current = GridBlocks.Find(x => x.GridPos == pos);
            if (current)
            {
                GridBlocks.Remove(current);
                Destroy(current.gameObject);
            }

            // Calculate the position of block based on its index and tileSize and instantiate the block prefab at the calculated position
            var blockPosition = new Vector3( pos.x * blockSize.x - offsetX, pos.y * blockSize.y - offsetY, 0.0f);
            var blockGo = Instantiate(blockPrefab, transform);
            blockGo.transform.localPosition = blockPosition;

            var excludedTypes = new List<BlockType>();
            if(playerId == PlayerId.Player)
                excludedTypes.Add(BlockType.MilitaryChest);
            
            // Update Tile Parameters and add new tile to the list.
            var blockScript = blockGo.GetComponent<Block>();
            blockScript.UpdateBlockProperties(SelectBlock(blockData.blockType, excludedTypes: excludedTypes), blockData.isSpawn);
            blockScript.GridPos = pos;
            blockScript.ID = id;
            blockGo.name = $"{blockScript.type}@({pos.x},{pos.y})#{id}";
            GridBlocks.Add(blockScript);
        }

        public bool HasSpawnSet()
        {
            var spawn = GridBlocks.Find(x => x.isSpawn);
            return spawn != null;
        }

        public int GetTotalWeight()
        {
            return GridBlocks.Sum(b => b.weight);
        }
    }
}
