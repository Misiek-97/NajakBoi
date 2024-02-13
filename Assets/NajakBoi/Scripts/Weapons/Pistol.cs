using UnityEngine;
using UnityEngine.EventSystems;

namespace NajakBoi.Scripts.Weapons
{
    public class Pistol : Weapon
    {
        
        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Instance.CanPlayerTakeAction(najakBoiController.playerId))
            {
                return;
            }
            /*
            najakBoiController.controller.isAiming = Input.GetMouseButton(1);
            if (najakBoiController.controller.isAiming)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    najakBoiController.controller.isAiming = false;
                    gameManager.playerController.AnimatePistolFire();
                    Fire();
                }
                Debug.DrawRay(projectileExit.transform.position, projectileExit.transform.forward * 100f, Color.red);
            }
            */
        }
    }
}
