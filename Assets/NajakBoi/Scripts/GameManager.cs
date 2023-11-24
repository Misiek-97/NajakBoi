using NajakBoi.Scripts.UI;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NajakBoi.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public EndGameScreen endGameScreen;

        public PlayerController player;
        public PlayerController opponent;

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
            playerTurn = PlayerId.Player;
            player.gameObject.SetActive(true);
            opponent.gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            Debug.Log(playerTurn);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.T))
                EndTurn();
        }

        public void EndTurn()
        {
            player.gameObject.SetActive(false);
            opponent.gameObject.SetActive(false);

            switch (playerTurn)
            {
                case PlayerId.Player:
                    playerTurn = PlayerId.Opponent;
                    opponent.gameObject.SetActive(true);
                    player.gameObject.SetActive(true);
                    break; 

                case PlayerId.Opponent: 
                    playerTurn = PlayerId.Player;
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

        public void PlayerDeath(PlayerId player)
        {
            endGameScreen.GameEnded(player);
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
