using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBar : Bar
{
    public PlayerController player;

    public void UpdateMovement()
    {
        amount = player.currentMovement;
        maxAmount = player.maxMovement;
    }
}
