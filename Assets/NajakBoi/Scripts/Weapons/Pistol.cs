using UnityEngine;
using UnityEngine.EventSystems;

namespace NajakBoi.Scripts.Weapons
{
    public class Pistol : Weapon
    {
        
        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Instance.CanPlayerTakeAction(playerController.playerId))
            {
                playerController.controller.isAiming = false;
                return;
            }

            playerController.controller.isAiming = Input.GetMouseButton(1);
            if (playerController.controller.isAiming)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    playerController.controller.isAiming = false;
                    gameManager.playerController.AnimatePistolFire();
                    Fire();
                }
                Debug.DrawRay(projectileExit.transform.position, projectileExit.transform.forward * 100f, Color.red);
            }
        }
    }
}
