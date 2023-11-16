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
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle between the current object position and the mouse position
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;

        // Rotate the object to face the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        if (Input.GetMouseButton(0))
        {
            _force += forceMultiplier * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            var pos = gameObject.transform.position;
            Vector3 dir = (Input.mousePosition - pos).normalized;

            var projectileInstance = Instantiate(projectilePrefab, barrel);
            projectileInstance.transform.parent = GameObject.Find("Canvas").transform;
            Debug.Log(projectileInstance.transform.position);
            var projectile = projectileInstance.GetComponent<Projectile>();
            projectile.rb.AddForce(dir * _force);


            _force = 0f;

        }
    }
}
