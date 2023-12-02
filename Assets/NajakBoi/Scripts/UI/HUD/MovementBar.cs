using NajakBoi.Scripts.Player;

namespace NajakBoi.Scripts.UI.HUD
{
    public class MovementBar : Bar
    {
        public PlayerController player;

        public void UpdateMovement()
        {
            if (!player) return;
            amount = player.currentMovement;
            maxAmount = player.maxMovement;
        }
    }
}
