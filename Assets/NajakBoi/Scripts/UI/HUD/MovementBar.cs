using NajakBoi.Scripts.Player;
using UnityEngine.Serialization;

namespace NajakBoi.Scripts.UI.HUD
{
    public class MovementBar : Bar
    {
        [FormerlySerializedAs("player")] public NajakBoiController najakBoi;

        public void UpdateMovement()
        {
            if (!najakBoi) return;
            amount = najakBoi.currentMovement;
            maxAmount = najakBoi.maxMovement;
        }
    }
}
