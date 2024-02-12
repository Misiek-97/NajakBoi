using System;
using NajakBoi.Scripts.Blocks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace NajakBoi.Scripts.UI.EditMode
{
    public class BlockMenuButton : MonoBehaviour
    {
        [DoNotSerialize]public Block block;
        public Image image;
        public TextMeshProUGUI nameTmp;
        private Button _button;

        public void SetUpButton()
        {
            image.sprite = block.sprite;
            nameTmp.text = block.type.ToString();
            _button = GetComponentInChildren<Button>();
        }

        private void Update()
        {
            if (block.type == BlockType.Empty)
                return;
            if (GameManager.Instance.playerTurn == PlayerId.Player1)
            {
                _button.interactable =  block.weight + EditMenuManager.Instance.currentWeight <= EditMenuManager.Instance.maxWeight;
            }
            else
            {
                _button.interactable = true;
            }
        }

        public void Clicked()
        {
            BlockMenu.Instance.blockToPlace = block;
            EditMenuManager.Instance.UpdateBlockInfoText();
        }
    }
}
