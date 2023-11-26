using UnityEngine;

namespace NajakBoi.Scripts.HealKits
{
    public class Bandage : MonoBehaviour
    {
        public float healingAmount = 15f;
        public int available = 3;
        public PlayerController player;

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.H))
            {
                if (GameManager.Instance.editMode ||
                    player.playerId != GameManager.Instance.playerTurn ||
                    available <= 0 || 
                    player.currentHealth >= player.maxHealth) return;

                UseBandage();
            }

        }

        void UseBandage()
        {
            available--;
            player.GetHealed(healingAmount);
            GameManager.Instance.EndTurn();
        }
    }
}