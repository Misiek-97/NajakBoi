using System;
using System.Linq;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.Weapons;
using UnityEditor;
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

        [NonSerialized] public int NewAmmo;
        [NonSerialized] public float DamageMultiplier;
        [NonSerialized] public float MaxForceMultiplier;
        [NonSerialized] public float ExplosionRadiusMultiplier;

        public bool CanUpgrade()
        {
            var playerData = SessionManager.PlayerData;
            return requiredPlayerLevel <= playerData.Stats.Level && requiredResources.All(reqRes =>
                reqRes.amount <= playerData.Resources[reqRes.resourceType].amount);
        }

        public void SetName()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            var newName = $"WUT-{weaponType}-{upgradeType}-{level}";

            AssetDatabase.RenameAsset(assetPath, newName);
        }
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
            
            if (wut.upgradeType == UpgradeType.Ammo)
            {
                wut.NewAmmo = EditorGUILayout.IntField("New Ammo", wut.NewAmmo);
            }
            
            if (wut.upgradeType == UpgradeType.Damage)
            {
                wut.DamageMultiplier = EditorGUILayout.FloatField("Damage Multiplier", wut.DamageMultiplier);
            }
            
            if (wut.upgradeType == UpgradeType.MaxForce)
            {
                wut.MaxForceMultiplier = EditorGUILayout.FloatField("Max Force Multiplier", wut.MaxForceMultiplier);
            }
            
            if (wut.upgradeType == UpgradeType.ExplosionRadius)
            {
                wut.ExplosionRadiusMultiplier = EditorGUILayout.FloatField("Explosion Radius Multiplier", wut.ExplosionRadiusMultiplier);
            }
            
        }
    }
#endif
}
