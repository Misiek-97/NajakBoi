using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int hitPoints;   
    public Sprite sprite;
    public Image image;
    public int id;
    public TileType type;
    public GameObject tileMenuPrefab;
    public Vector2 gridPos;

    public BoxCollider2D col;

    private int _currentHitPoints;


    private void Awake()
    {
        if (type == TileType.Empty)
        {
            col.enabled = false;
        }
        
        _currentHitPoints = hitPoints;

        if(sprite != null )
            image.sprite = sprite;
    }


    public void GetDamaged(int dmg)
    {
        _currentHitPoints -= dmg;
        
        if(_currentHitPoints <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            var projectile = collision.gameObject.GetComponent<Projectile>();

            GetDamaged(projectile.damage);
            Destroy(collision.gameObject);
        }
    }
 
    public void UpdateTile(Tile tile)
    {
        hitPoints = tile.hitPoints;
        sprite = tile.sprite;
        type = tile.type;
        id = tile.id;
        
        image.color = Color.white;
        image.sprite = sprite;
        _currentHitPoints = hitPoints;
        col.enabled = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(0, 255, 0, 100);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    { 
        image.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!TileMenu.Instance)
            Instantiate(tileMenuPrefab, GameObject.Find("Canvas").transform);
        else
            TileMenu.Instance.transform.position = Input.mousePosition;
        
        TileMenu.TileBeingEdited = this;
    }

}
