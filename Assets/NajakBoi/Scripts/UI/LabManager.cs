using System.Text;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NajakBoi.Scripts.UI
{
    public class LabManager : MonoBehaviour
    {

        public TextMeshProUGUI resourcesTmp;
        // Start is called before the first frame update
        void Start()
        {
            DisplayResources();
        }

        private void DisplayResources()
        {
            var resources = SessionManager.PlayerData.Resources;

            var sb = new StringBuilder();
            sb.Append("RESOURCES\r\n");
            foreach (var r in resources)
            {
                sb.AppendLine($"{r.Key.ToString()}: {r.Value.amount}");
            }

            resourcesTmp.text = sb.ToString();
        }

        public void AddCoin()
        {
            SessionManager.PlayerData.GainResource(ResourceType.Coins, 1);
            DisplayResources();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
