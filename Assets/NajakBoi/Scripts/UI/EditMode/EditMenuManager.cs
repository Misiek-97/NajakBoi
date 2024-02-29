using System;
using System.Text;
using NajakBoi.Scripts.Session;
using TMPro;
using UnityEngine;

namespace NajakBoi.Scripts.UI.EditMode
{
    public class EditMenuManager : MonoBehaviour
    {
        public TextMeshProUGUI placementInfoTmp;
        public TextMeshProUGUI infoTmp;
        public TextMeshProUGUI weightTmp;
        public TextMeshProUGUI blockInfoTmp;

        public int currentWeight;
        public int maxWeight;
        
        public static bool SetSpawn;
        public static EditMenuManager Instance;

        private void Awake()
        {
            if (Instance)
            {
                Debug.Log("Session already exists!");
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            maxWeight = SessionManager.PlayerData.BuildingStats.maxWeight;
        }


        public void OnDestroy()
        {
            if (Instance != this) return;
            
            Instance = null;
        }

        private void Start()
        {
            UpdateWeightText();
        }


        // Update is called once per frame
        void Update()
        {
            if (SetSpawn)
            {
                placementInfoTmp.text = "Click on a block to set as spawn...";
                return;
            }
            
            var sb = new StringBuilder();
            sb.Append(BlockMenu.BlockBeingEdited
                ? $"Editing: {BlockMenu.BlockBeingEdited.type} @ {BlockMenu.BlockBeingEdited.GridPos}"
                : "Editing: N/A");


            sb.AppendLine(BlockMenu.Instance.blockToPlace
                ? $"\r\nBlock To Place: {BlockMenu.Instance.blockToPlace.type}"
                : "\r\nBlock To Place: N/A");

            placementInfoTmp.text = sb.ToString();
        }

        public void UpdateInfoText(string text)
        {
            infoTmp.text = text;
        } 
        public void UpdateWeightText()
        {
            currentWeight = GameManager.Instance.player1Grid.GetTotalWeight();
            weightTmp.text = $"Blocks Weight: {currentWeight} / {maxWeight}";
        }
        public void UpdateBlockInfoText()
        {
            var sb = new StringBuilder();
            var block = BlockMenu.Instance.blockToPlace;
            if (block == null)
            {
                blockInfoTmp.text = "Select a block for more information.";
                return;
            }
            sb.Append($"Type: {block.type}");
            sb.Append($"\r\nHitPoints: {block.maxHealth}");
            sb.Append($"\r\nWeight: {block.weight}");
            
            blockInfoTmp.text = sb.ToString();
        }

        public void EndEditTurn()
        {
            if (GameManager.Instance.playerTurn.Value == PlayerId.Player1)
            {
                if (GameManager.Instance.player1Grid.HasSpawnSet())
                {
                    GameManager.Instance.EndEdit();
                    infoTmp.text = "";
                }
                else
                {
                    infoTmp.text = "You must set the spawn point before ending edit turn!";
                }
            }
            else
            {
                if (GameManager.Instance.player2Grid.HasSpawnSet())
                {
                    GameManager.Instance.EndEdit();
                    infoTmp.text = "";
                }
                else
                {
                    infoTmp.text = "You must set the spawn point before ending edit turn!";
                }
            }
        }

        public void ToggleSetSpawn()
        {
            SetSpawn = !SetSpawn;
        }

        public void RandomizeGrid()
        {
            if (GameManager.Instance.playerTurn.Value == PlayerId.Player1)
            {
                GameManager.Instance.player1Grid.CreateGrid(true);
                UpdateWeightText();
            }
            else
            {
                GameManager.Instance.player2Grid.CreateGrid(true);
            }
        }

        public void StartEditTurn(PlayerId playerId)
        {
            SetAnchoringFor(playerId);
            UpdateBlockInfoText();
            if (playerId == PlayerId.Player1)
            {
               UpdateWeightText();
            }
            else
            {
                weightTmp.text = "";
            }
        }

        private void SetAnchoringFor(PlayerId playerId)
        {
            var rt = GetComponent<RectTransform>();
            switch (playerId)
            {
                case PlayerId.Player1:
                    rt.anchorMin = new Vector2(1f, 1f);
                    rt.anchorMax = new Vector2(1f, 1f);
                    rt.pivot = new Vector2(1f, 1f);
                    rt.anchoredPosition = Vector3.zero;
                    break;
                case PlayerId.Player2:
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
