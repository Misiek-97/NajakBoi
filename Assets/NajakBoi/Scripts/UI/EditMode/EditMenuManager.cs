using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NajakBoi.Scripts.UI.EditMode
{
    public class EditMenuManager : MonoBehaviour
    {
        public TextMeshProUGUI placementInfoTmp;
        public static bool SetSpawn;
       
        // Update is called once per frame
        void Update()
        {
            var sb = new StringBuilder();
            sb.Append(BlockMenu.BlockBeingEdited
                ? $"Editing: {BlockMenu.BlockBeingEdited.type} @ {BlockMenu.BlockBeingEdited.GridPos}"
                : "Editing: N/A");


            sb.AppendLine(BlockMenu.Instance.blockToPlace
                ? $"Block To Place: {BlockMenu.Instance.blockToPlace.type}"
                : "Block To Place: N/A");

            placementInfoTmp.text = sb.ToString();
            
        }

        public void EndEditTurn()
        {
            if (GameManager.Instance.playerTurn == PlayerId.Player)
            {
                if (GameManager.Instance.playerGrid.HasSpawnSet())
                {
                    GameManager.Instance.EndEdit();
                }
                else
                {
                    placementInfoTmp.text = "You must set the spawn point before ending edit turn!";
                }
            }
            else
            {
                if (GameManager.Instance.opponentGrid.HasSpawnSet())
                {

                    GameManager.Instance.EndEdit();
                }
                else
                {
                    placementInfoTmp.text = "You must set the spawn point before ending edit turn!";
                }
            }
        }

        public void ToggleSetSpawn()
        {
            SetSpawn = !SetSpawn;
        }

        public void StartEditTurn(PlayerId playerId)
        {
            SetAnchoringFor(playerId);
        }

        private void SetAnchoringFor(PlayerId playerId)
        {
            var rt = GetComponent<RectTransform>();
            switch (playerId)
            {
                case PlayerId.Player:
                    rt.anchorMin = new Vector2(1f, 1f);
                    rt.anchorMax = new Vector2(1f, 1f);
                    rt.pivot = new Vector2(1f, 1f);
                    rt.anchoredPosition = Vector3.zero;
                    break;
                case PlayerId.Opponent:
                    rt.anchorMin = new Vector2(0f, 1f);
                    rt.anchorMax = new Vector2(0f, 1f);
                    rt.pivot = new Vector2(0f, 1f);
                    rt.anchoredPosition = Vector3.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerId), playerId, null);
            }
        }
    }
}
