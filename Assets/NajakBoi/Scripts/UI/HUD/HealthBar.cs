using NajakBoi.Scripts.Player;

namespace NajakBoi.Scripts.UI.HUD
{
    public class HealthBar : Bar
    {
        public PlayerController player;


        public void UpdateHealth()
        {
            if (!player) return;
            amount = player.currentHealth;
            maxAmount = player.maxHealth;
        }

    }
}
