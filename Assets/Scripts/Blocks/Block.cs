using System;
using UnityEngine;
using Weapons;

namespace Blocks
{
    public class Block : MonoBehaviour
    {
        public GameObject blockMenuPrefab;
        public Material mat;
        public GameObject highlightGo;
        private BoxCollider _collider;
        private MeshRenderer _renderer;
    
        public int hitPoints;   
        public Sprite sprite;
        public int id;
        public BlockType type;
        public Vector2 gridPos;

        private int _currentHitPoints;

        void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material = mat;

            _collider = GetComponent<BoxCollider>();
            
            _currentHitPoints = hitPoints;
        }

        public void GetDamaged(int dmg)
        {
            _currentHitPoints -= dmg;
        
            if(_currentHitPoints <= 0)
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
            hitPoints = block.hitPoints;
            sprite = block.sprite;
            type = block.type;
            mat = block.mat;
            id = block.id;
            
            _renderer.material = mat;
            if (!GameManager.Instance.editMode && type == BlockType.Empty)
                _renderer.enabled = false;
            
            _currentHitPoints = hitPoints;
            gameObject.name = $"{type}@{gridPos}#{id}";
            gameObject.layer = type == BlockType.Empty ? LayerMask.NameToLayer("IgnoreCollision") : 0;
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.editMode) return;
            
            /*
            if (!BlockMenu.Instance)
                Instantiate(blockMenuPrefab, GameObject.Find("Canvas").transform);
            else
                BlockMenu.Instance.transform.position = Input.mousePosition;
        */
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
