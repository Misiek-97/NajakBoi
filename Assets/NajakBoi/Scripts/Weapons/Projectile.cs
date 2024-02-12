using UnityEngine;

namespace NajakBoi.Scripts.Weapons
{
    public class Projectile : MonoBehaviour
    {
        public float damage;
        public Rigidbody2D rb;
        public float lifetime;
        public float explosionRadius;
        public float explosionForce;
        public GameObject explosionFx;

        private float _currentLifetime;

        private void Start()
        {
            Physics.IgnoreLayerCollision(gameObject.layer,  LayerMask.NameToLayer("IgnoreCollision"));
        }

        private void Update()
        {
            _currentLifetime += 1 * Time.deltaTime;
            if (_currentLifetime >= lifetime)
                Destroy(gameObject);
        }
       

        private void Explosion()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach (Collider2D col in colliders)
            {
                // Check if the collider has a component that can take damage
                IDamageable damageable = col.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    // Apply damage to the object
                    damageable.GetDamaged(damage);
                    damageable.ApplyExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            // Destroy the explosion prefab after applying damage
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red; // Set the color of the wire sphere

            // Draw a wire sphere around the object to represent the explosion radius
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        private void OnDestroy()
        {
            Explosion();
            var fx = Instantiate(explosionFx);
            fx.transform.position = transform.position;
            fx.transform.localScale = fx.transform.localScale * (explosionRadius * 2);
        }
    }
}
