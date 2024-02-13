using NajakBoi.Scripts.Player;

namespace NajakBoi.Scripts.UI.HUD
{
    public class HealthBar : Bar
    {
        public NajakBoiController najakBoi;


        public void UpdateHealth()
        {
            if (!najakBoi) return;
            amount = najakBoi.currentHealth;
            maxAmount = najakBoi.maxHealth;
        }

    }
}
