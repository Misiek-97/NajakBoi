using System;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
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
     
        public Sprite sprite;
        public GameObject blockCanvasPrefab;
        public GameObject highlightGo;
        
        [NonSerialized]public MeshRenderer Renderer;
        [NonSerialized]public Vector2 GridPos;
        [NonSerialized]public int ID;
        private BlockHealthBar _healthBar;
        private GameObject _canvas;

        void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();
            
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
            currentHealth -= dmg;
            _healthBar.UpdateHealth();
            _canvas.SetActive(true);
            if(currentHealth <= 0)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            var loot = lootTable.GenerateLoot();
            foreach (var item in loot)
            {
                Debug.Log($"{gameObject.name} Dropped {item.Amount} {item.Type}(s)");
                SessionManager.PlayerData.GainResource(item.Type, item.Amount);
            }
        }

        private void GenerateLoot()
        {
            
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
            
            Renderer.material = mat;
            if (!GameManager.Instance.editMode && type == BlockType.Empty && GameManager.Instance.editMode)
            {
                Renderer.enabled = false;
                _canvas.SetActive(false);
            }

            if (mesh)
                GetComponent<MeshFilter>().mesh = mesh;

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
