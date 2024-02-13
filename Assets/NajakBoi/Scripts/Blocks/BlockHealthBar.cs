using NajakBoi.Scripts.UI.HUD;
using UnityEngine;

namespace NajakBoi.Scripts.Blocks
{
    public class BlockHealthBar : Bar
    {
        public Block block;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _canvas.worldCamera = Camera.main;
        }

        public void UpdateHealth()
        {
            amount = block.CurrentHealth;
            maxAmount = block.maxHealth;
        }
    }
}
