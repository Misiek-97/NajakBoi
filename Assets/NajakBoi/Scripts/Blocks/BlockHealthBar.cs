using NajakBoi.Scripts.UI.HUD;
using UnityEngine;

namespace NajakBoi.Scripts.Blocks
{
    public class BlockHealthBar : Bar
    {
        private Block _block;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = Camera.main;
            _block = GetComponentInParent<Block>();
        }

        public void UpdateHealth()
        {
            amount = _block.currentHealth;
            maxAmount = _block.maxHealth;
        }
    }
}
