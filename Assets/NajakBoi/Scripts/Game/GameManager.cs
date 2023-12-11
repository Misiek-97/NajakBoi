using System;
using System.Collections;
using System.Collections.Generic;
using NajakBoi.Scripts.Blocks;
using NajakBoi.Scripts.Player;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.UI;
using NajakBoi.Scripts.UI.EditMode;
using StarterAssets;
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
        public CameraManager cameraManager;

        public PlayerController player;
        public PlayerController opponent;

        public BlockSpawner playerGrid;
        public BlockSpawner opponentGrid;

        public PlayerId playerTurn;
        public ThirdPersonController playerController;
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
                endGameScreen.GameEnded(PlayerId.Opponent);
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
            playerTurn = PlayerId.Player;
            if (GameMode == GameMode.LocalPvP)
                EditingStage();
        }

        public void StartGame()
        {
            playerTurn = PlayerId.Player;
            turnInfoTmp.text = $"{playerTurn.ToString()}'s Turn";
            editMode = false;
            
            hud.SetActive(true);
            editCanvas.SetActive(false);
            
            playerGrid.RefreshAllBlocks();
            opponentGrid.RefreshAllBlocks();

            var playerSpawnPoint = playerGrid.GridBlocks.Find(x => x.isSpawn).transform.position;
            player.transform.position = playerSpawnPoint + new Vector3(0, 0.5f, 0);
            player.gameObject.SetActive(true);
            
            if (GameMode == GameMode.Expedition)
            {
                opponent.healthBar.gameObject.SetActive(false);
                opponent.movementBar.gameObject.SetActive(false);
                return;
            }
            
            var opponentSpawnPoint = opponentGrid.GridBlocks.Find(x => x.isSpawn).transform.position;
            opponent.transform.position = opponentSpawnPoint + new Vector3(0, 0.5f, 0);
            opponent.gameObject.SetActive(true);
        }

        public bool IsMyTurn(PlayerId playerId) => playerId == playerTurn;

        private void EditingStage()
        {
            turnInfoTmp.text = $"Editing Stage {playerTurn.ToString()}'s Turn";
            playerTurn = PlayerId.Player;
            editMode = true;
            hud.SetActive(false);
            editMenu.gameObject.SetActive(true);
            editMenu.StartEditTurn(playerTurn);
        }

        public void EndEdit()
        {
            switch (playerTurn)
            {
                case PlayerId.Player:
                    playerTurn = PlayerId.Opponent;
                    editMenu.StartEditTurn(playerTurn);
                    playerGrid.SaveGrid();
                    turnInfoTmp.text = $"Editing Stage {playerTurn.ToString()}'s Turn";
                    break;
                case PlayerId.Opponent:
                    playerTurn = PlayerId.Player;
                    opponentGrid.SaveGrid();
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
            player.gameObject.SetActive(false);
            opponent.gameObject.SetActive(false);

            switch (playerTurn)
            {
                case PlayerId.Player:
                    playerController = opponent.GetComponent<ThirdPersonController>();
                    playerTurn = PlayerId.Opponent;
                    opponent.gameObject.SetActive(true);
                    player.gameObject.SetActive(true);
                    break; 

                case PlayerId.Opponent: 
                    playerTurn = PlayerId.Player;
                    playerController = player.GetComponent<ThirdPersonController>();
                    player.gameObject.SetActive(true);
                    opponent.gameObject.SetActive(true);
                    break;
            }

            player.currentMovement = player.maxMovement;
            opponent.currentMovement = opponent.maxMovement;
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
                SceneManager.LoadScene("Game");
        }

        public void PlayerDeath(PlayerId playerId)
        {
            turnInfoTmp.text = "Game Over!";
            endGameScreen.GameEnded(playerId);
        }

        public void RestartGame()
        {
            playerTurn = PlayerId.Player;
            SceneManager.LoadScene("Game");
        }
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    
    }
}
