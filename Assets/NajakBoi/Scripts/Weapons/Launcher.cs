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
            if (!GameManager.Instance.CanPlayerTakeAction(najakBoiController.playerId))
            {
                najakBoiController.chargeBar.SetFillAmount(0f);
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
                _force = Mathf.Clamp(_force += forceMultiplier * Time.deltaTime, minForce, maxForce);

                najakBoiController.chargeBar.SetFillAmount(_force / maxForce);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Fire(_force);
                _force = minForce;

                najakBoiController.chargeBar.SetFillAmount(_force);
                najakBoiController.chargeBar.gameObject.SetActive(false);
            }
        }
    }
}
