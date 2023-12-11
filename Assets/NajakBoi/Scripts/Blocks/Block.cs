using System;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.UI;
using NajakBoi.Scripts.UI.EditMode;
using NajakBoi.Scripts.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace NajakBoi.Scripts.Blocks
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Block : MonoBehaviour, IDamageable
    {
        public BlockType type;
        public float maxHealth;
        public float currentHealth;
        public Material mat;
        public Mesh mesh;
        public LootTable lootTable;
        public GameObject dropDisplayPrefab;
        public Vector3 offset;
     
        public Sprite sprite;
        public GameObject blockCanvasPrefab;
        public GameObject highlightGo;
        
        [NonSerialized]public MeshRenderer Renderer;
        [NonSerialized]public Vector2 GridPos;
        [NonSerialized]public int ID;
        private BlockHealthBar _healthBar;
        private BoxCollider _collider;
        private GameObject _canvas;

        public bool isSpawn;

        private BlockSpawner _blockSpawner;

        void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<BoxCollider>();
            _blockSpawner = GetComponentInParent<BlockSpawner>();
            currentHealth = maxHealth;
            if(blockCanvasPrefab)
            {
                _canvas = Instantiate(blockCanvasPrefab, transform);
                _canvas.transform.localPosition = new Vector3(0f, 0f, -0.51f);
                _canvas.SetActive(false);
                _healthBar = _canvas.GetComponent<BlockHealthBar>();                
            }
        
        }

        public void GetDamaged(float dmg)
        {
            if (type == BlockType.Empty) return;
            
            currentHealth -= dmg;
            _healthBar.UpdateHealth();
            _canvas.SetActive(true);
            if (currentHealth <= 0)
            {
                if (lootTable)
                {
                    GenerateLoot();
                }
                else
                {
                    Debug.LogWarning($"No Loot Table set up on {gameObject.name}!");
                }
                
                Destroy(gameObject);
            }
        }

        private void GenerateLoot()
        {
            if (lootTable == null) return;
            var display = Instantiate(dropDisplayPrefab).GetComponent<DropDisplay>();
            display.gameObject.transform.position = transform.position;
            var loot = lootTable.GenerateLoot();
            foreach (var item in loot)
            {
                SessionManager.PlayerData.GainResource(item.Type, item.Amount);
            }

            display.DisplayDrops(loot);
        }

        public void ApplyExplosionForce(float force, Vector3 origin, float radius)
        {
            //Do nothing for blocks
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                var projectile = collision.gameObject.GetComponent<Projectile>();

                GetDamaged(projectile.damage);
                Destroy(collision.gameObject);
            }
        }
        
        //EDIT MODE
 
        public void UpdateBlockProperties(Block block, bool spawn)
        {
            maxHealth = block.maxHealth;
            sprite = block.sprite;
            type = block.type;
            mat = block.mat;
            ID = block.ID;
            mesh = block.mesh;
            lootTable = block.lootTable;
            dropDisplayPrefab = block.dropDisplayPrefab;
            offset = block.offset;
            isSpawn = spawn;
            
            Renderer.material = mat;
            if (!GameManager.Instance.editMode && type == BlockType.Empty)
            {
                Renderer.enabled = false;
                _canvas.SetActive(false);
            }

            if (mesh)
            {
                GetComponent<MeshFilter>().mesh = mesh;
                _collider.size = mesh.bounds.size;
                _collider.center = new Vector3(0f, mesh.bounds.size.y / 2f, 0f);
            }
            
            transform.localPosition += offset;

            currentHealth = maxHealth;

            if (_healthBar)
                _healthBar.UpdateHealth();

            gameObject.name = $"{type}@{GridPos}#{ID}";
            gameObject.layer = type == BlockType.Empty ? LayerMask.NameToLayer("IgnoreCollision") : 0;
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.editMode) return;
            
            BlockMenu.BlockBeingEdited = this;

            if (EditMenuManager.SetSpawn)
            {
                if (type != BlockType.Empty)
                {
                    var blockAbove = _blockSpawner.GridBlocks.Find(x => x.GridPos == new Vector2(GridPos.x, GridPos.y + 1));
                    var blockAbove2 = _blockSpawner.GridBlocks.Find(x => x.GridPos == new Vector2(GridPos.x, GridPos.y + 2));

                    if ((blockAbove == null || blockAbove.type == BlockType.Empty) &&
                        (blockAbove2 == null || blockAbove2.type == BlockType.Empty))
                    {
                        var prevSpawn = _blockSpawner.GridBlocks.FindAll(x => x.isSpawn);
                        foreach (var b in prevSpawn)
                        {
                            b.isSpawn = false;
                            Debug.Log($"{b.type} at {b.GridPos} removed as spawn.");
                        }
                        
                        EditMenuManager.Instance.UpdateInfoText($"{type} at {GridPos} set as spawn.");
                        isSpawn = true;
                        EditMenuManager.SetSpawn = false;
                        _blockSpawner.SaveGrid();
                    }
                    else
                    {
                        EditMenuManager.Instance.UpdateInfoText($"Unable to set spawn here! Make sure there are none or 2 empty blocks above {type} at {GridPos} to set spawn.");
                    }
                }
                else
                {
                    EditMenuManager.Instance.UpdateInfoText($"Unable to set spawn on an empty block!");
                }
                return;
            }
            
            if (BlockMenu.Instance.blockToPlace)
            {
                if (BlockMenu.Instance.blockToPlace.type != BlockType.Empty)
                {
                    var blockBelow = _blockSpawner.GridBlocks.Find(x => x.GridPos == new Vector2(GridPos.x, GridPos.y - 1));
                    var blockBelow2 = _blockSpawner.GridBlocks.Find(x => x.GridPos == new Vector2(GridPos.x, GridPos.y - 2));
                    if (blockBelow && blockBelow.isSpawn)
                    {
                        EditMenuManager.Instance.UpdateInfoText($"Can't place {BlockMenu.Instance.blockToPlace.type} here, as block below is reserved for spawn!");
                        return;
                    }
                    
                    if(blockBelow2 && blockBelow2.isSpawn)
                    {
                        EditMenuManager.Instance.UpdateInfoText($"Can't place {BlockMenu.Instance.blockToPlace.type} here, as blocks below are reserved for spawn!");
                        return;
                    }
                }

                UpdateBlockProperties(BlockMenu.Instance.blockToPlace, false);
            }
        }

        private void OnMouseEnter()
        {
            if (!GameManager.Instance.editMode) return;
            
            highlightGo.SetActive(true);
        }
        
        private void OnMouseExit()
        {
            if (!GameManager.Instance.editMode) return;
            
            highlightGo.SetActive(false);
        }
    }
}
