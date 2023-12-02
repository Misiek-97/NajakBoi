using System;
using System.Linq;
using NajakBoi.Scripts.Blocks;
using NajakBoi.Scripts.Player;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Upgrading;
using UnityEngine;
using UnityEngine.Serialization;

namespace NajakBoi.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public float damage;
        public float minForce;
        public float maxForce;
        public float explosionRadius;
        public float fireRate;
        public WeaponType weaponType;
        public FireMode fireMode;
        public bool useAmmo;
        public int ammo;
        public GameObject model;
        public GameObject projectilePrefab;
        public Transform projectileExit;
        public PlayerController playerController;
        public GameManager gameManager => GameManager.Instance;

        private void Awake()
        {
            playerController = GetComponentInParent<PlayerController>();
            GetWeaponUpgrades();
        }

        private void GetWeaponUpgrades()
        {
            var wutManager = SessionManager.Session.wutManager;
            var weapon = SessionManager.PlayerData.Weapons[weaponType];
            var ammoWut = wutManager.GetWutFor(weaponType, UpgradeType.Ammo, weapon.ammoData.level);
            if (ammoWut)
            {
                Debug.Log($"Set {weaponType} Ammo to {(int)ammoWut.value}");
                ammo = (int)ammoWut.value;
            }
            
            var damageWut = wutManager.GetWutFor(weaponType, UpgradeType.Damage, weapon.damageData.level);
            if (damageWut)
            {
                damage *= damageWut.value;
                Debug.Log($"Set {weaponType} Damage to {damage}");
            }
            
            var forceWut = wutManager.GetWutFor(weaponType, UpgradeType.MaxForce, weapon.forceData.level);
            if (forceWut)
            {
                maxForce *= forceWut.value;
                Debug.Log($"Set {weaponType} MaxForce to {maxForce}");
            }

            var explosionWut = wutManager.GetWutFor(weaponType, UpgradeType.ExplosionRadius, weapon.explosionData.level);
            if (explosionWut)
            {
                explosionRadius *= explosionWut.value;
                Debug.Log($"Set {weaponType} Explosion Radius to {explosionRadius}");
            }
        }


        public void Fire(float force = 0f)
        {
            if (useAmmo && ammo <= 0)
            {
                Debug.Log($"No Ammo Left in {gameObject.name}");
                return;
            }

            ammo--;
            
            switch (fireMode)
            {
                case FireMode.Automatic:
                    break;
                case FireMode.SemiAuto:
                    PistolFire();
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

        private void PistolFire()
        {
            var origin = projectileExit.transform.position;
            var radius = 0.5f;
            var distance = 100f;
            var direction = projectileExit.forward;
            
            if (Physics.SphereCast(origin, radius, direction, out RaycastHit hitInfo, distance))
            {
                // A collision occurred, and hitInfo now contains information about the hit
                Debug.DrawRay(origin, direction * hitInfo.distance, Color.red);

                var player = hitInfo.collider.gameObject.GetComponent<PlayerController>();
                if (player)
                    player.GetDamaged(damage);

                var block = hitInfo.collider.gameObject.GetComponent<Block>();
                if(block)
                    block.GetDamaged(damage);
            }
            else
            {
                // No collision
                Debug.DrawRay(origin, direction * distance, Color.green);
            }
            
            GameManager.Instance.EndTurn();
        }

        private WeaponUpgradeTable GetUpgrade(UpgradeType upType)
        {
            var wpn = SessionManager.PlayerData.Weapons[weaponType];
            var wutList = SessionManager.Session.wutManager.weaponUpgradeTables;
            var sortedWutList = wutList.Where(x => x.upgradeType == upType && x.weaponType == weaponType).ToList();

            foreach (var wut in sortedWutList)
            {
                //TODO: Add WeaponUpgradeData to WeaponData   
            }

            return null;
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
            var col = instance.GetComponent<Collider>();
            projectile.explosionRadius = explosionRadius;
            projectile.damage = damage;

            Physics.IgnoreCollision(col, playerController.GetComponent<Collider>());
            
            //Ensure Projectile and Direction is at 0z
            direction = new Vector3(direction.x, direction.y, 0f);
            instance.transform.position = new Vector3(pExit.x, pExit.y, 0.0f);

            // Apply force to the Rigidbody in the calculated direction
            projectile.rb.AddForce(direction * force, ForceMode.Impulse);
            GameManager.Instance.playerController.isAiming = false;
            GameManager.Instance.EndTurn();
        }
    }
}
