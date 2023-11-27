using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NajakBoi.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
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
