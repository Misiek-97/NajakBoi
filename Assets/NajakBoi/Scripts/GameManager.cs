using System;
using NajakBoi.Scripts.Blocks;
using NajakBoi.Scripts.UI;
using NajakBoi.Scripts.UI.EditMode;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NajakBoi.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public EndGameScreen endGameScreen;
        public GameObject hud;
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


        private void Start()
        {
            EditingStage();
            playerTurn = PlayerId.Player;
        }

        private void StartGame()
        {
            playerTurn = PlayerId.Player;
            editMode = false;
            
            hud.SetActive(true);
            editCanvas.SetActive(false);
            
            playerGrid.RefreshAllBlocks();
            opponentGrid.RefreshAllBlocks();
            
            player.gameObject.SetActive(true);
            opponent.gameObject.SetActive(true);
        }

        private void EditingStage()
        {
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

        public void EndTurn()
        {
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
