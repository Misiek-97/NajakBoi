using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileMenuButton : MonoBehaviour
{
    [FormerlySerializedAs("tile")] public Block block;
    public Image image;
    public TextMeshProUGUI nameTmp;

    public void SetUpButton()
    {
        image.sprite = block.sprite;
        nameTmp.text = block.type.ToString();
    }

    public void Clicked()
    {
        TileMenu.BlockBeingEdited.UpdateBlockProperties(block);
        TileMenu.Instance.Close();
    }
    
    
}
