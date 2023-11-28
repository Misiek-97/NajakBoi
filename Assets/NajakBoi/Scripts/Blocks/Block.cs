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

        void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<BoxCollider>();
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
            Debug.Log($"Generating Loot after destroying {gameObject.name}");
            var display = Instantiate(dropDisplayPrefab).GetComponent<DropDisplay>();
            display.gameObject.transform.position = transform.position;
            var loot = lootTable.GenerateLoot();
            foreach (var item in loot)
            {
                Debug.Log($"{gameObject.name} Dropped {item.Amount} {item.Type}(s)");
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
 
        public void UpdateBlockProperties(Block block)
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
            
            Renderer.material = mat;
            if (!GameManager.Instance.editMode && type == BlockType.Empty && GameManager.Instance.editMode)
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
            if(BlockMenu.Instance.blockToPlace)
                UpdateBlockProperties(BlockMenu.Instance.blockToPlace);
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
