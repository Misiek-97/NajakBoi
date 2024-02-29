using System;
using System.Linq;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.Weapons;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace NajakBoi.Scripts.Systems.Upgrading
{
    [CreateAssetMenu(fileName = "WeaponUpgradeTable", menuName = "Custom/Weapon Upgrade Table")]
    public class WeaponUpgradeTable : ScriptableObject
    {
        [Header("Upgrade Definition")]
        public WeaponType weaponType;
        public UpgradeType upgradeType;
        public int level;
        
        [Header("Requirements")]
        public int requiredPlayerLevel;
        public Resource[] requiredResources;

        [Header("Grants")] 
        public float value;
        
        public bool CanUpgrade()
        {
            var playerData = SessionManager.PlayerData;
            return requiredPlayerLevel <= playerData.PlayerStats.Level && requiredResources.All(reqRes =>
                reqRes.amount <= playerData.Resources[reqRes.resourceType].amount);
        }
#if UNITY_EDITOR
        public void SetName()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            var newName = $"WUT-{weaponType}-{upgradeType}-{level}";

            AssetDatabase.RenameAsset(assetPath, newName);
        }
#endif
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(WeaponUpgradeTable))]
    public class WeaponUpgradeTableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var wut = (WeaponUpgradeTable)target;

            if (GUILayout.Button("Set Name"))
            {
                wut.SetName();
            }
        }
    }
#endif
}
