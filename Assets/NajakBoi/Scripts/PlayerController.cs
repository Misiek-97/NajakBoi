using System.Collections;
using NajakBoi.Scripts.UI.HUD;
using NajakBoi.Scripts.Weapons;
using StarterAssets;
using UnityEngine;

namespace NajakBoi.Scripts
{
    public class PlayerController : MonoBehaviour, IDamageable
    {

        public PlayerId playerId;
        public HealthBar healthBar;
        public MovementBar movementBar;
        public ChargeBar chargeBar;
        public float maxHealth = 100f;
        public float currentHealth;
        public float currentMovement;
        public float maxMovement = 100f;

        public ThirdPersonController controller;

        private bool _isDead;


        private void Start()
        {
            currentHealth = maxHealth;

            healthBar.player = this;
            healthBar.UpdateHealth();

            currentMovement = maxMovement;

            movementBar.player = this;
            movementBar.UpdateMovement();
        }


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                var p = other.gameObject.GetComponent<Projectile>();
                GetDamaged(p.damage);
                Destroy(p.gameObject);
            }
        } 
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("DeathZone"))
            {
                GetDamaged(currentHealth);
            }
        }

        public bool UseMovement(float amount)
        {
            currentMovement -= amount;
            movementBar.UpdateMovement();
            if(currentMovement <= 0f)
            {           
                GameManager.Instance.EndTurn();
                return true;
            }
            return false;
        }

        public void GetDamaged(float dmg)
        {
            currentHealth -= dmg;

            healthBar.UpdateHealth();

            if (currentHealth <= 0)
            {
                currentHealth = 0f;
                _isDead = true;
                GameManager.Instance.PlayerDeath(playerId);
            }
        }

        /*
        public void ApplyExplosionForce(float explosionForce, Vector3 origin, float explosionRadius)
        {
            var cc = GetComponent<CharacterController>();
            // Calculate direction from the explosion to the characters
            var pos = transform.position;
            var direction = (pos - origin).normalized;

            // Calculate explosion force
            var distance = Vector3.Distance(origin, pos);
            var force = Mathf.Clamp01(1f - distance / explosionRadius) * explosionForce;

            // Apply the force to the character controller
            cc.Move(direction * force + Vector3.up * 0.5f);

        }*/
        
        public void ApplyExplosionForce(float explosionForce, Vector3 origin, float explosionRadius)
        {
            var cc = GetComponent<CharacterController>();
            var pos = transform.position;
            var direction = (pos - origin).normalized;
            var distance = Vector3.Distance(origin, pos);
            var force = Mathf.Clamp01(1f - distance / explosionRadius) * explosionForce;

            // Calculate the target position after applying the force
            var targetPosition = pos + (direction * force + Vector3.up * 0.5f);

        }
    }
}
