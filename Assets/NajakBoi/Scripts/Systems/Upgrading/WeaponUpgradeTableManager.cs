using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NajakBoi.Scripts.Weapons;
using UnityEditor;
using UnityEngine;

namespace NajakBoi.Scripts.Systems.Upgrading
{
    public class WeaponUpgradeTableManager : MonoBehaviour
    {
        public List<WeaponUpgradeTable> weaponUpgradeTables = new List<WeaponUpgradeTable>();
        public string searchFolder = "Assets/NajakBoi/UpgradeTables"; // Specify your folder path

        private void Awake()
        {
            PopulateWeaponUpgradeTables();
        }
        [ContextMenu("Populate Weapon Upgrade Tables")]
        public void PopulateWeaponUpgradeTables()
        {
            weaponUpgradeTables.Clear();

            string[] guids = AssetDatabase.FindAssets("t:WeaponUpgradeTable", new[] { searchFolder });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                WeaponUpgradeTable weaponUpgradeTable = AssetDatabase.LoadAssetAtPath<WeaponUpgradeTable>(assetPath);

                if (weaponUpgradeTable != null)
                {
                    weaponUpgradeTables.Add(weaponUpgradeTable);
                }
            }

            Debug.Log("Weapon Upgrade Tables populated. Count: " + weaponUpgradeTables.Count);
        }
        
        [CanBeNull]
        public WeaponUpgradeTable GetWutFor(WeaponType wpnType, UpgradeType upType, int level)
        {
            var wut = weaponUpgradeTables.Find(x =>
                x.weaponType == wpnType && x.upgradeType == upType && x.level == level);
            return wut;
        }
        

#if UNITY_EDITOR
        [CustomEditor(typeof(WeaponUpgradeTableManager))]
        public class WeaponUpgradeTableManagerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                var manager = (WeaponUpgradeTableManager)target;

                GUILayout.Space(10);

                if (GUILayout.Button("Populate Weapon Upgrade Tables"))
                {
                    manager.PopulateWeaponUpgradeTables();
                }
            }
        }
#endif
    }
}