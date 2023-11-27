using System;
using System.IO;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public static class GlobalSession
    {
        public static PlaySession Session;

        public static string ResourcesPath => Path.Combine(Application.persistentDataPath, "/Resources.json");
        public static string PlayerGridPath => Path.Combine(Application.persistentDataPath, "/PlayerBlockGrid.json");
        public static string OpponentGridPath => Path.Combine(Application.persistentDataPath, "/OpponentBlockGrid.json");

        public static volatile bool loaded = false;

        private static object lockObject = new object();
        private static bool saving;

        static GlobalSession()
        {
            Session = new PlaySession();
            if(!Init(ResourcesPath))
            {
                Session.Player = new PlayerData();
                Session.Player.PopulateResourcesDictionary();
                Debug.Log("GLOBAL-SESSION - New play session started, and Populated Resources");
            }

            loaded = true;
        }

        public static bool Init(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Session.Player.LoadPlayerData();
                    Debug.Log($"GLOBAL-SESSION - Save loaded from {path}");
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex is NotSupportedException)
                        Debug.Log("GLOBAL-SESSION - Cleared Save as part of a Migration");

                    Debug.Log($"GLOBAL-SESSION - Save corrupt at {path}");
                    Debug.LogError(ex);
                    return false;
                }
            }

            return false;
        }

        public static void Save()
        {
            lock (lockObject)
            {
                if (saving)
                    return;

                saving = true;
            }

            try
            {
                Session.Player.SavePlayerData();
                Debug.Log("GLOBAL-SESSION - Player Data Saved");
            }
            finally
            {
                lock (lockObject)
                {
                    saving = false;
                }
            }
        }

        public static void ClearSave()
        {
            lock (lockObject)
            {
                if (File.Exists(ResourcesPath))
                    File.Delete(ResourcesPath);

                if (File.Exists(PlayerGridPath))
                    File.Delete(PlayerGridPath);
                
                if (File.Exists(OpponentGridPath))
                    File.Delete(OpponentGridPath);

                Session.Player = new PlayerData();
            }
        }
    }
}

