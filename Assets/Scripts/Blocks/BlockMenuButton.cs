using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Blocks
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
            BlockMenu.BlockBeingEdited.UpdateBlockProperties(block);
            BlockMenu.Instance.Close();
        }
    
    
    }
}
