using Blocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
