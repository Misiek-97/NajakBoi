using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
          
    public GameObject blockMenuPrefab;
    public Material mat;
    private BoxCollider _collider;
    private MeshRenderer _renderer;
    
    
    public int hitPoints;   
    public Sprite sprite;
    public Image image;
    public int id;
    public BlockType type;
    public GameObject tileMenuPrefab;
    public Vector2 gridPos;

    private int _currentHitPoints;

    
    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = mat;
        
        
        _collider = GetComponent<BoxCollider>();
        if (type == BlockType.Empty)
        {
            _collider.enabled = false;
        }
        
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
        
        _currentHitPoints = hitPoints;

        _collider.enabled = block.type != BlockType.Empty;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!TileMenu.Instance)
            Instantiate(tileMenuPrefab, GameObject.Find("Canvas").transform);
        else
            TileMenu.Instance.transform.position = Input.mousePosition;
        
        TileMenu.BlockBeingEdited = this;
    }

}
