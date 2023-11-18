using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileMenuButton : MonoBehaviour
{
    public Tile tile;
    public Image image;
    public TextMeshProUGUI nameTmp;

    public void SetUpButton()
    {
        image.sprite = tile.sprite;
        nameTmp.text = tile.type.ToString();
    }

    public void Clicked()
    {
        TileMenu.TileBeingEdited.UpdateTile(tile);
        TileMenu.Instance.Close();
    }
    
    
}
