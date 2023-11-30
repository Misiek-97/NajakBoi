using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Levelling;
using NajakBoi.Scripts.Systems.Upgrading;
using NajakBoi.Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NajakBoi.Scripts.UI
{
    public class LabManager : MonoBehaviour
    {
        public TextMeshProUGUI resourcesTmp;
        public TextMeshProUGUI statsTmp;
        public TextMeshProUGUI upgradesTmp;
        public Toggle startToggle;
        public List<GameObject> panels;
        private int nextLevelXp;
        private UpgradeManager _upgradeManager;

        public static LabManager Instance;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance && Instance == this)
            {
                Instance = null;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _upgradeManager = GetComponent<UpgradeManager>();
            nextLevelXp = ExperienceManager.CalculateExperienceToNextLevel();
            startToggle.SetIsOnWithoutNotify(true);
            startToggle.onValueChanged.Invoke(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                var nextLevel = ExperienceManager.GainExperience(100);
                if (nextLevel != 0)
                {
                    nextLevelXp = ExperienceManager.CalculateExperienceToNextLevel();
                }
            }
        }

        public void AddAllResources()
        {
            SessionManager.PlayerData.AddAllResources();
            DisplayResources();
        } 
        public void ClearAllResources()
        {
            SessionManager.PlayerData.ClearAllResources();
            DisplayResources();
        }

        private void DisplayResources()
        {
            var resources = SessionManager.PlayerData.Resources;

            var sb = new StringBuilder();
            sb.Append("Resources\r\n");
            foreach (var r in resources)
            {
                sb.AppendLine($"{r.Key.ToString()}: {r.Value.amount}");
            }

            resourcesTmp.text = sb.ToString();
        }

        private void DisplayUpgrades()
        {
            var weapons = SessionManager.PlayerData.Weapons;

            var sb = new StringBuilder();
            sb.Append("Weapons\r\n");
            foreach(var wpn in weapons)
            {
                if (wpn.Key is WeaponType.Rifle or WeaponType.Sniper) continue;
                
                sb.AppendLine($"\r\n<b>{wpn.Key} Upgrades</b>");
                sb.AppendLine($"Damage Level: {wpn.Value.damageData.level}");
                
                if (wpn.Key == WeaponType.Pistol) continue;
                sb.AppendLine($"Ammo Level: {wpn.Value.ammoData.level}");
                sb.AppendLine($"Force Level: {wpn.Value.forceData.level}");
                sb.AppendLine($"Explosion Level: {wpn.Value.explosionData.level}");
            }

            upgradesTmp.text = sb.ToString();
            _upgradeManager.ValidateUpgradeButtons();
        }   

        private void DisplayNajakBoi()
        {
            var stats = SessionManager.PlayerData.Stats;

            var sb = new StringBuilder();
            sb.Append("Najak Boi Statistics\r\n");
            
            sb.AppendLine("\r\nLevel & Experience");
            sb.AppendLine($"Level: {stats.Level}");
            sb.AppendLine($"Experience: {stats.Experience}/{nextLevelXp}");

            sb.AppendLine("\r\nGeneral");
            sb.AppendLine($"Max Health: {stats.MaxHealth}");
            sb.AppendLine($"Max Movement: {stats.MaxMovement}");
            sb.AppendLine($"Max Jump Height: {stats.MaxJumpHeight}");
            
            sb.AppendLine("\r\nResistances");
            sb.AppendLine($"Blast Resistance: {stats.BlastResistance}");
            sb.AppendLine($"Pierce Resistance: {stats.PierceResistance}");
            sb.AppendLine($"Fire Resistance: {stats.FireResistance}");
            sb.AppendLine($"Cold Resistance: {stats.ColdResistance}");
            sb.AppendLine($"Lightning Resistance: {stats.LightningResistance}");
            sb.AppendLine($"Poison Resistance: {stats.PoisonResistance}");

            statsTmp.text = sb.ToString();
        }

        public void DisplayPanelByName(string panelName)
        {
            DisableAll();
            var panel = panels.FirstOrDefault(x => x.name.Contains(panelName));
            if (!panel) throw new InvalidOperationException($"Could not find selected panel {panelName}!");
            panel.SetActive(true);
            
            switch (panelName)
            {
                case "NajakBoi": 
                    DisplayNajakBoi();
                    break;
                case "Resources":
                    DisplayResources();
                    break;
                case "Arsenal":
                    break;
                case "Upgrade":
                    DisplayUpgrades();
                    break;
                case "Research":
                    break;
                default:
                    throw new InvalidOperationException($"Panel {panelName} was not found!");
            }
        }

        private void DisableAll()
        {
            foreach(var p in panels)
                p.SetActive(false);
        }
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
