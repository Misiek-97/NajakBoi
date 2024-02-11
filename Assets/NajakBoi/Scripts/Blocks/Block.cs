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
    [RequireComponent(typeof(SpriteRenderer))]
    public class Block : MonoBehaviour, IDamageable
    {
        public BlockType type;
        public float maxHealth;
        public LootTable lootTable;
        public Vector3 offset;
        public int weight;
        
        public Sprite sprite;
        public GameObject highlightGo;
        public BlockHealthBar healthBar;
        public GameObject canvas;
     
        private BoxCollider2D _collider;
        
        [NonSerialized]public float CurrentHealth;
        [NonSerialized]public SpriteRenderer Renderer;
        [NonSerialized]public Vector2 GridPos;
        [NonSerialized]public int ID;

        public bool isSpawn;

        private BlockSpawner _blockSpawner;

        void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
            _blockSpawner = GetComponentInParent<BlockSpawner>();
            healthBar.block = this;
            CurrentHealth = maxHealth;
        
        }

        public void GetDamaged(float dmg)
        {
            if (type == BlockType.Empty) return;
            
            CurrentHealth -= dmg;
            healthBar.UpdateHealth();
            canvas.SetActive(true);
            if (CurrentHealth <= 0)
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
            var loot = lootTable.GenerateLoot();
            foreach (var item in loot)
            {
                SessionManager.PlayerData.GainResource(item.Type, item.Amount);
            }
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
            ID = block.ID;
            lootTable = block.lootTable;
            offset = block.offset;
            weight = block.weight;
            isSpawn = spawn;
            
            if (/*!GameManager.Instance.editMode &&*/ type == BlockType.Empty)
            {
                Renderer.enabled = false;
                canvas.SetActive(false);
            }

            
            transform.localPosition += offset;

            CurrentHealth = maxHealth;

            if (healthBar)
                healthBar.UpdateHealth();

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

            var blockMenu = BlockMenu.Instance;
            var editMenu = EditMenuManager.Instance;
            var btp = blockMenu.blockToPlace;
            if (btp)
            {
                if (btp.type != BlockType.Empty)
                {
                    if (btp.weight + editMenu.currentWeight > editMenu.maxWeight)
                    {
                        editMenu.UpdateInfoText($"Can't place {btp.type} because it exceeds maximum block weight!");
                        blockMenu.blockToPlace = null;
                        return;
                    }
                    
                    var blockBelow = _blockSpawner.GridBlocks.Find(x => x.GridPos == new Vector2(GridPos.x, GridPos.y - 1));
                    var blockBelow2 = _blockSpawner.GridBlocks.Find(x => x.GridPos == new Vector2(GridPos.x, GridPos.y - 2));
                    if (blockBelow && blockBelow.isSpawn)
                    {
                        editMenu.UpdateInfoText($"Can't place {btp.type} here, as block below is reserved for spawn!");
                        return;
                    }
                    
                    if(blockBelow2 && blockBelow2.isSpawn)
                    {
                        editMenu.UpdateInfoText($"Can't place {btp.type} here, as blocks below are reserved for spawn!");
                        return;
                    }
                }
                else
                {
                    if (isSpawn)
                    {
                        editMenu.UpdateInfoText("This block is used as spawn point and can't be made empty!");
                        return;
                    }
                }

                UpdateBlockProperties(btp, false);
                editMenu.UpdateInfoText($"Successfully placed {btp.type} at {GridPos}.");
                editMenu.UpdateWeightText();
            }
        }

        private void OnMouseEnter()
        {
            //if (!GameManager.Instance.editMode) return;
            
            highlightGo.SetActive(true);
        }
        
        private void OnMouseExit()
        {
            //if (!GameManager.Instance.editMode) return;
            
            highlightGo.SetActive(false);
        }
    }
}
