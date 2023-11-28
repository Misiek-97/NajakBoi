using UnityEngine;

namespace NajakBoi.Scripts
{
    public interface IDamageable
    {
        void GetDamaged(float damage);

        void ApplyExplosionForce(float force, Vector3 origin, float radius);
    }
}

