using UnityEngine;

namespace Weapons
{
    public class Launcher : Weapon
    {
        public float forceMultiplier;
        private float _force;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                GameManager.Instance.playerController.isAiming = true;
                _force += forceMultiplier * Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Fire(_force);
                _force = 0f;
            }
        }
    }
}
