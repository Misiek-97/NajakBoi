using System;
using System.IO;
using NajakBoi.Scripts.Systems.Upgrading;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public class SessionManager : MonoBehaviour
    {
        private static DateTime _lastSave = DateTime.Now;
        public static PlayerData PlayerData;
        public static string ResourcesPath => Path.Combine(Application.persistentDataPath, "Resources.json");

        public static SessionManager Session;

        public WeaponUpgradeTableManager wutManager;

        public const int MaxBuildX = 15;
        public const int MaxBuildY = 10;
        public const int MaxBuildWeight = 200;
        

        private void Awake()
        {
            if (Session)
            {
                Debug.Log("Session already exists!");
                Destroy(gameObject);
                return;
            }
            
            Session = this;
            DontDestroyOnLoad(gameObject);
        }


        public void OnDestroy()
        {
            if (Session != this) return;
            
            Session = null;
            Save();
        }

        private void Start()
        {
            wutManager = GetComponent<WeaponUpgradeTableManager>();
            PlayerData = new PlayerData();
            PlayerData.LoadPlayerData();
        }

        public static void Save()
        {
            PlayerData.SavePlayerData();
        }


        private static void NewSession()
        {
            PlayerData = new PlayerData();
            PlayerData.PopulateResourcesDictionary();
            Debug.Log("NEW PLAY SESSION");
        }
        private void Update()
        {
            if (_lastSave < DateTime.Now.AddMinutes(-1))
            {
                _lastSave = DateTime.Now;
                Debug.Log($"AUTOSAVE {DateTime.Now}");
                Save();
            }
        }
        

    }
}
