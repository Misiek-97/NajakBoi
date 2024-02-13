using System.Text;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Levelling;
using TMPro;
using UnityEngine;

namespace NajakBoi.Scripts.UI
{
    public class EndGameScreen : MonoBehaviour
    {
        public TextMeshProUGUI endTextTmp;
        public TextMeshProUGUI summaryTmp;

        // Start is called before the first frame update
        void Awake()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void GameEnded(PlayerId looser)
        {
            var xpGained = looser == PlayerId.Player2 ? 100 : 50;
            
            ExperienceManager.GainExperience(xpGained);
            
            var sb = new StringBuilder();
            sb.Append("Summary\r\n");
            sb.AppendLine($"Level: {SessionManager.PlayerData.PlayerStats.Level}");
            sb.AppendLine($"Experience: {SessionManager.PlayerData.PlayerStats.Experience} / {ExperienceManager.CalculateExperienceToNextLevel()}");
            
            var resources = GameManager.Instance.CalculateEndResources();
            if (resources.Count > 0)
            {
                sb.AppendLine("\r\nResources Gained:");
                foreach(var r in resources)
                    sb.AppendLine($"+{r.amount} {r.resourceType}");
            }
            
            summaryTmp.text = sb.ToString();
            if (GameManager.GameMode == GameMode.Expedition)
            {
                endTextTmp.text = looser == PlayerId.Player2 ? "Expedition Successful!" : "Expedition Failed!";
                endTextTmp.color = looser == PlayerId.Player2 ? Color.green : Color.red;
            }
            else
            {
                endTextTmp.text = looser == PlayerId.Player2 ? "You Won!" : "You Lost!";
                endTextTmp.color = looser == PlayerId.Player2 ? Color.green : Color.red;
            }
            gameObject.SetActive(true);
        }
    }
}
