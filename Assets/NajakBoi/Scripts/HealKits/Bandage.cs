using NajakBoi.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace NajakBoi.Scripts.HealKits
{
    public class Bandage : MonoBehaviour
    {
        public float healingAmount = 15f;
        public int available = 3;
        [FormerlySerializedAs("player")] public NajakBoiController najakBoi;

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.H))
            {
                if (GameManager.Instance.editMode ||
                    najakBoi.playerId != GameManager.Instance.playerTurn ||
                    available <= 0 || 
                    najakBoi.currentHealth >= najakBoi.maxHealth) return;

                UseBandage();
            }

        }

        void UseBandage()
        {
            available--;
            najakBoi.GetHealed(healingAmount);
            GameManager.Instance.EndTurn();
        }
    }
}