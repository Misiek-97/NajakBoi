using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public ThirdPersonController playerController;
    public bool editMode;

    public GameObject editCanvas;

    public static GameManager Instance;

    public LayerMask ignoreCollision;
    public LayerMask blockLayer;

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
