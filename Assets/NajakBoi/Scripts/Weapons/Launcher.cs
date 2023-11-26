using UnityEngine;
using UnityEngine.EventSystems;

namespace NajakBoi.Scripts.Weapons
{
    public class Launcher : Weapon
    {
        public float minForce;
        public float maxForce;

        public float forceMultiplier;
        private float _force;

        private void Start()
        {
            _force = minForce;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameManager.editMode || playerController.playerId != gameManager.playerTurn || EventSystem.current.IsPointerOverGameObject()) return;
            
            if (Input.GetMouseButton(0))
            {
                gameManager.playerController.isAiming = true;
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
