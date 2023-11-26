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

        public void SetUpButton()
        {
            image.sprite = block.sprite;
            nameTmp.text = block.type.ToString();
        }

        public void Clicked()
        {
            BlockMenu.Instance.blockToPlace = block;
        }
    }
}
