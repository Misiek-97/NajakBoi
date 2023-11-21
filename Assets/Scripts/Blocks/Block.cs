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
 
        public void UpdateBlockProperties(Block block)
        {
            hitPoints = block.hitPoints;
            sprite = block.sprite;
            type = block.type;
            mat = block.mat;
            id = block.id;
            
            _renderer.material = mat;
            _currentHitPoints = hitPoints;
        }

        private void OnMouseDown()
        {
            if (!BlockMenu.Instance)
                Instantiate(blockMenuPrefab, GameObject.Find("Canvas").transform);
            else
                BlockMenu.Instance.transform.position = Input.mousePosition;
        
            BlockMenu.BlockBeingEdited = this;
        }

        private void OnMouseEnter()
        {
            highlightGo.SetActive(true);
        }
        
        private void OnMouseExit()
        {
            highlightGo.SetActive(false);
        }
    }
}
