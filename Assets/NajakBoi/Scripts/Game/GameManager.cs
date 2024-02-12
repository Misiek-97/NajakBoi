using System;
using System.Collections;
using System.Collections.Generic;
using NajakBoi.Scripts.Blocks;
using NajakBoi.Scripts.Player;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.UI;
using NajakBoi.Scripts.UI.EditMode;
using SupanthaPaul;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace NajakBoi.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public EndGameScreen endGameScreen;
        public GameObject hud;
        public TextMeshProUGUI turnInfoTmp;
        public EditMenuManager editMenu;
        public CameraFollow cameraFollow;

        public NajakBoiController player1;
        public NajakBoiController player2;

        public BlockSpawner player1Grid;
        public BlockSpawner player2Grid;

        public PlayerId playerTurn;
        public bool editMode;

        public GameObject editCanvas;

        public static GameManager Instance;
        public static bool EndingTurn;
        public static GameMode GameMode;

        private Dictionary<ResourceType, Resource> _startResources = new();

        private void Awake()
        {
            if (Instance)
            {
                Debug.Log("GameManager Instance already exists!");
                Destroy(gameObject);
                return;
            }
            
            
        
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && GameMode == GameMode.Expedition)
                endGameScreen.GameEnded(PlayerId.Player2);
        }

        private void StoreStartResources()
        {
            foreach (var entry in SessionManager.PlayerData.Resources)
            {
                _startResources.Add(entry.Key, entry.Value);
            }
        }
        
        public List<Resource> CalculateEndResources()
        {
            var endResources = new List<Resource>();
            foreach (var entry in SessionManager.PlayerData.Resources)
            {
                var gainedAmount = entry.Value.amount - _startResources[entry.Key].amount;
                if (gainedAmount <= 0) continue;
                var res = new Resource()
                {
                    amount = gainedAmount,
                    resourceType = entry.Key
                };
                endResources.Add(res);
            }

            return endResources;
        }


        private void Start()
        {
            StoreStartResources();
            playerTurn = PlayerId.Player1;
            if (GameMode == GameMode.LocalPvP)
                EditingStage();
        }

        public void StartGame()
        {
            playerTurn = PlayerId.Player1;
            turnInfoTmp.text = $"{playerTurn.ToString()}'s Turn";
            editMode = false;
            
            hud.SetActive(true);
            editCanvas.SetActive(false);
            
            player1Grid.RefreshAllBlocks();
            player2Grid.RefreshAllBlocks();

            var player1SpawnPoint = player1Grid.GridBlocks.Find(x => x.isSpawn).transform.position;
            player1.transform.position = player1SpawnPoint + new Vector3(0, 1f, 0);
            player1.controller.enabled = true;
            player1.gameObject.SetActive(true);

            var player2SpawnPoint = player2Grid.GridBlocks.Find(x => x.isSpawn).transform.position;
            player2.transform.position = player2SpawnPoint + new Vector3(0, 1f, 0);
            player2.controller.enabled = false;
            player2.gameObject.SetActive(true);
            
            if (GameMode == GameMode.Expedition)
            {
                player2.healthBar.gameObject.SetActive(false);
                player2.movementBar.gameObject.SetActive(false);
            }
            
        }

        public bool IsMyTurn(PlayerId playerId) => playerId == playerTurn;

        private void EditingStage()
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(false);
            turnInfoTmp.text = $"Editing Stage {playerTurn.ToString()}'s Turn";
            playerTurn = PlayerId.Player1;
            editMode = true;
            hud.SetActive(false);
            editMenu.gameObject.SetActive(true);
            editMenu.StartEditTurn(playerTurn);
        }

        public void EndEdit()
        {
            switch (playerTurn)
            {
                case PlayerId.Player1:
                    playerTurn = PlayerId.Player2;
                    editMenu.StartEditTurn(playerTurn);
                    player1Grid.SaveGrid();
                    turnInfoTmp.text = $"Editing Stage {playerTurn.ToString()}'s Turn";
                    break;
                case PlayerId.Player2:
                    playerTurn = PlayerId.Player1;
                    player2Grid.SaveGrid();
                    StartGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator EndTurnDelayed()
        {
            EndingTurn = true;
            turnInfoTmp.text = $"Ending {playerTurn.ToString()}'s Turn";
            yield return new WaitForSeconds(2f);

            switch (playerTurn)
            {
                case PlayerId.Player1:
                    playerTurn = PlayerId.Player2;
                    player2.controller.enabled = true;
                    player1.controller.enabled = false;
                    cameraFollow.target = player2.transform;
                    break; 

                case PlayerId.Player2: 
                    playerTurn = PlayerId.Player1;
                    player2.controller.enabled = false;
                    player1.controller.enabled = true;
                    cameraFollow.target = player1.transform;
                    break;
            }

            player1.currentMovement = player1.maxMovement;
            player2.currentMovement = player2.maxMovement;
            turnInfoTmp.text = $"{playerTurn.ToString()}'s Turn";
            EndingTurn = false;
        }

        public bool CanPlayerTakeAction(PlayerId playerId)
        {
            return !editMode && !EndingTurn && IsMyTurn(playerId) && !EventSystem.current.IsPointerOverGameObject();
        }
        
        public void EndTurn()
        {
            if(GameMode != GameMode.Expedition)
                StartCoroutine(EndTurnDelayed());
        }

        public void EditMode()
        {
            editMode = !editMode;
            editCanvas.SetActive(editMode);

            if (!editMode)
                SceneManager.LoadScene("Game-2D");
        }

        public void PlayerDeath(PlayerId playerId)
        {
            turnInfoTmp.text = "Game Over!";
            endGameScreen.GameEnded(playerId);
        }

        public void RestartGame()
        {
            playerTurn = PlayerId.Player1;
            SceneManager.LoadScene("Game-2D");
        }
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    
    }
}
