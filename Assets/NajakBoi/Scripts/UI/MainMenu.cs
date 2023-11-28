using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NajakBoi.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
      
        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        } 
        public void GoToLab()
        {
            SceneManager.LoadScene("TheLab");
        }

        public void StartLocalPvE()
        {
            GameManager.GameMode = GameMode.PvE;
            StartGame();
        }   
        
        public void StartExpedition()
        {
            GameManager.GameMode = GameMode.Expedition;
            StartGame();
        } 
        
        public void StartLocalPvP()
        {
            GameManager.GameMode = GameMode.LocalPvP;
            StartGame();
        }
    
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
        }
    
    }
}
