using System;
using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        public int damage;
        public float fireRate;
        public WeaponType type;
        public FireMode fireMode;
        public GameObject model;
        public GameObject projectilePrefab;
        public Transform projectileExit;


        public void Fire(float force)
        {
            switch (fireMode)
            {
                case FireMode.Automatic:
                    break;
                case FireMode.SemiAuto:
                    break;
                case FireMode.Burst:
                    break;
                case FireMode.BoltAction:
                    break;
                case FireMode.Launcher:
                    LauncherFire(force);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LauncherFire(float force)
        {
            if (projectilePrefab == null)
                throw new Exception($"Projectile Model not set on Launcher {gameObject.name}!");

            if (projectileExit == null)
                throw new Exception($"Projectile Exit not set on Launcher {gameObject.name}");

            var pExit = projectileExit.position;

            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Set a fixed depth in world space
            mousePosition.z = Vector3.Distance(Camera.main.transform.position, pExit); 

            // Convert the mouse position to a world position
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Calculate the direction from pExit to the mouse position
            Vector3 direction = targetPosition - pExit;

            // Normalize the direction to get a unit vector
            direction.Normalize();

            // Create a new projectile at pExit position
            var instance = Instantiate(projectilePrefab);
            var projectile = instance.GetComponent<Projectile>();
            
            //Ensure Projectile and Direction is at 0z
            direction = new Vector3(direction.x, direction.y, 0f);
            instance.transform.position = new Vector3(pExit.x, pExit.y, 0.0f);

            // Apply force to the Rigidbody in the calculated direction
            projectile.rb.AddForce(direction * force, ForceMode.Impulse);
            GameManager.Instance.playerController.isAiming = false;
        }
    }
}
