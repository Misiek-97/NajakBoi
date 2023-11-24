using System;
using System.IO;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public static class GlobalSession
    {
        public static PlaySession Session;

        public static string SavePath => Path.Combine(Application.persistentDataPath, "save.najak");
        public static string BackupPath => SavePath + ".backup";

        public static volatile bool loaded = false;

        private static object lockObject = new object();
        private static bool saving;

        static GlobalSession()
        {
            Session = new PlaySession();
            if (!Init(SavePath) && !Init(BackupPath))
            {
                Session.Player = new PlayerData();
                Debug.Log("GLOBAL-SESSION - New play session started");
            }

            loaded = true;
        }

        static bool Init(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Session.Player = PlayerData.LoadFromFile(path);
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
                var path = SavePath;
                Session.Player.SaveToFile(BackupPath);
                Session.Player.SaveToFile(path);
                Debug.Log($"GLOBAL-SESSION - Saved to {path}");
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
                if (File.Exists(SavePath))
                    File.Delete(SavePath);

                if (File.Exists(BackupPath))
                    File.Delete(BackupPath);

                Session.Player = new PlayerData();
            }
        }
    }
}

