using System;
using NajakBoi.Scripts.Systems.Economy;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public class SessionManager : MonoBehaviour
    {
        private static DateTime _lastSave = DateTime.Now;
        
        private void Start()
        {
            GlobalSession.Save();
            
            GlobalSession.Session.Player.GainResource(ResourceType.Coins, 10);
        }
        private void Update()
        {
            if (_lastSave < DateTime.Now.AddMinutes(-1))
            {
                _lastSave = DateTime.Now;
                GlobalSession.Save();
            }
        }

        public void OnDestroy()
        {
            GlobalSession.Save();
        }
    }
}
