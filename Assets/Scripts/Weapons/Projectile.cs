using Blocks;
using UnityEngine;

namespace Weapons
{
    public class Projectile : MonoBehaviour
    {
        public int damage;
        public Rigidbody rb;
        public float lifetime;

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
       
    }
}
