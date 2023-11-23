using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public GameObject player;
    public GameObject opponent;
    public bool playerTurn;
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

    private void Start()
    {
        playerTurn = true;
        player.SetActive(true);
        opponent.SetActive(true);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
            EndTurn();
    }

    public void EndTurn()
    {
        playerTurn = !playerTurn;
        
        player.SetActive(false);
        opponent.SetActive(false);

        if (playerTurn)
        {
            player.SetActive(true);
            opponent.SetActive(true);
        }
        else
        {
            opponent.SetActive(true);
            player.SetActive(true);
        }
    }

    public void EditMode()
    {
        editMode = !editMode;
        editCanvas.SetActive(editMode);

        if (!editMode)
            SceneManager.LoadScene("Game");
    }

    public void PlayerDeath()
    {
        gameOverScreen.SetActive(true);
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
