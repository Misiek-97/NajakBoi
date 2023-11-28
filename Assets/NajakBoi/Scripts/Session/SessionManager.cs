using System;
using System.IO;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public class SessionManager : MonoBehaviour
    {
        private static DateTime _lastSave = DateTime.Now;
        public static PlayerData PlayerData;
        public static string ResourcesPath => Path.Combine(Application.persistentDataPath, "Resources.json");

        public static SessionManager Session;
        

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
            if (File.Exists(ResourcesPath))
            {
                PlayerData = new PlayerData();
                try
                {
                    PlayerData.LoadPlayerData();
                }
                catch (Exception ex)
                {
                    Debug.Log($"Save corrupt at {ResourcesPath}!");
                    Debug.LogError(ex);
                }
            }
            else
            {
                NewSession();
            }
        }

        public void Save()
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
                Save();
            }
        }

    }
}
