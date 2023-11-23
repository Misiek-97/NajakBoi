using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;

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
        if(player == PlayerId.Player)
        {
            gameOverScreen.SetActive(true);
        }

        if(player == PlayerId.Opponent)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
