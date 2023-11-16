using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform barrel;
    public float forceMultiplier;

    private float _force;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _force += forceMultiplier * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var pos = gameObject.transform.position;
            Vector3 dir = (Input.mousePosition - pos).normalized;

            var projectileInstance = Instantiate(projectilePrefab, barrel);
            var projectile = projectileInstance.GetComponent<Projectile>();
            projectile.rb.AddForce(dir * _force);

            _force = 0f;

        }
    }
}
