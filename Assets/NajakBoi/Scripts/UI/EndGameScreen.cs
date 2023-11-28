using System.Text;
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
            var xpGained = looser == PlayerId.Opponent ? 100 : 50;
            
            ExperienceManager.GainExperience(xpGained);
            
            var sb = new StringBuilder();
            sb.Append("Summary\r\n");
            sb.AppendLine($"Experience Gained: {xpGained}");
            summaryTmp.text = sb.ToString();
            endTextTmp.text = looser == PlayerId.Opponent ? "You Won!" : "You Lost!";
            endTextTmp.color = looser == PlayerId.Opponent ? Color.green : Color.red;
            gameObject.SetActive(true);
        }
    }
}
