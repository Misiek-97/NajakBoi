using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public Rigidbody2D rb;
    public float lifetime;

    private float _currentLifetime;

    private void Update()
    {
        _currentLifetime += 1 * Time.deltaTime;
        if (_currentLifetime >= lifetime)
            Destroy(gameObject);
    }
}
