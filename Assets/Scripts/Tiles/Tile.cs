using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int hitpoints;   
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;
    public Collider2D col;

    private int _currentHitpoints;


    private void Awake()
    {
        _currentHitpoints = hitpoints;

        if(sprite != null )
            spriteRenderer.sprite = sprite;
    }


    public void GetDamaged(int dmg)
    {
        _currentHitpoints -= dmg;

        Debug.Log(_currentHitpoints);

        if(_currentHitpoints <= 0)
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


}