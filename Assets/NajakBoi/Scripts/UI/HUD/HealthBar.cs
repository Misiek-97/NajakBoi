using NajakBoi.Scripts.Player;
using UnityEngine.Serialization;

namespace NajakBoi.Scripts.UI.HUD
{
    public class HealthBar : Bar
    {
        [FormerlySerializedAs("player")] public NajakBoiController najakBoi;


        public void UpdateHealth()
        {
            if (!najakBoi) return;
            amount = najakBoi.currentHealth;
            maxAmount = najakBoi.maxHealth;
        }

    }
}
