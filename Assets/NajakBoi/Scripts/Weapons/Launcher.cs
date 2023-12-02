using UnityEngine;
using UnityEngine.EventSystems;

namespace NajakBoi.Scripts.Weapons
{
    public class Launcher : Weapon
    {
        public float forceMultiplier;
        private float _force;

        private void Start()
        {
            _force = minForce;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Instance.CanPlayerTakeAction(playerController.playerId))
            {
                playerController.controller.isAiming = false;
                playerController.chargeBar.SetFillAmount(0f);
                _force = minForce;
                return;
            }

            if (ammo <= 0)
            {
                Debug.Log($"No Ammo Left in {gameObject.name}");
                return;
            }
            
            if (Input.GetMouseButton(0))
            {
                playerController.controller.isAiming = true;
                _force = Mathf.Clamp(_force += forceMultiplier * Time.deltaTime, minForce, maxForce);

                playerController.chargeBar.SetFillAmount(_force / maxForce);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Fire(_force);
                _force = minForce;

                playerController.chargeBar.SetFillAmount(_force);
                playerController.chargeBar.gameObject.SetActive(false);
            }
        }
    }
}
