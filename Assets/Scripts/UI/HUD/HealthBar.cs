using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Bar
{
    public PlayerController player;


    public void UpdateHealth()
    {
        amount = player.currentHealth;
        maxAmount = player.maxHealth;
    }

}
