using NajakBoi.Scripts.UI.EditMode;
using NajakBoi.Scripts.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace NajakBoi.Scripts.Blocks
{
    public class Block : MonoBehaviour, IDamageable
    {
        public GameObject blockMenuPrefab;
        public Material mat;
        public GameObject highlightGo;
        public BlockHealthBar healthBar;
        public GameObject blockCanvasPrefab;
        private GameObject _canvas;

        private BoxCollider _collider;
        public MeshRenderer render;
     
        public Sprite sprite;
        public int id;
        public BlockType type;
        public Vector2 gridPos;

        public float maxHealth;
        public float currentHealth;

        void Awake()
        {
            render = GetComponent<MeshRenderer>();
            render.material = mat;

            _collider = GetComponent<BoxCollider>();
            
            currentHealth = maxHealth;
            if(blockCanvasPrefab)
            {
                _canvas = Instantiate(blockCanvasPrefab, transform);
                _canvas.transform.localPosition = new Vector3(0f, 0f, -0.51f);
                _canvas.SetActive(false);
                healthBar = _canvas.GetComponent<BlockHealthBar>();                
            }
        
        }

        public void GetDamaged(float dmg)
        {
            currentHealth -= dmg;
            healthBar.UpdateHealth();
            _canvas.SetActive(true);
            if(currentHealth <= 0)
                Destroy(gameObject);
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
            id = block.id;
            
            render.material = mat;
            if (!GameManager.Instance.editMode && type == BlockType.Empty && GameManager.Instance.editMode)
            {
                render.enabled = false;
                _canvas.SetActive(false);
            }

            currentHealth = maxHealth;

            if (healthBar)
                healthBar.UpdateHealth();

            gameObject.name = $"{type}@{gridPos}#{id}";
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
