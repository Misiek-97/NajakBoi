using System.Text;
using TMPro;
using UnityEngine;

namespace NajakBoi.Scripts.UI.EditMode
{
    public class EditMenuManager : MonoBehaviour
    {
        public TextMeshProUGUI placementInfoTmp;
       
        // Update is called once per frame
        void Update()
        {
            var sb = new StringBuilder();
            sb.Append(BlockMenu.BlockBeingEdited
                ? $"Editing: {BlockMenu.BlockBeingEdited.type} @ {BlockMenu.BlockBeingEdited.gridPos}"
                : "Editing: N/A");


            sb.AppendLine(BlockMenu.Instance.blockToPlace
                ? $"Block To Place: {BlockMenu.Instance.blockToPlace.type}"
                : "Block To Place: N/A");

            placementInfoTmp.text = sb.ToString();
            
        }
    }
}
