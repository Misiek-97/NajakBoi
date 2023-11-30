using System;
using System.Collections.Generic;
using System.Linq;
using NajakBoi.Scripts.Session;
using NajakBoi.Scripts.UI;
using NajakBoi.Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NajakBoi.Scripts.Systems.Upgrading
{
    public class UpgradeManager : MonoBehaviour
    {
        public TextMeshProUGUI selectedTmp;
        public WeaponType selectedWeapon;

        public Button upgradeAmmoBtn;
        public Button upgradeDamageBtn;
        public Button upgradeExplosionBtn;
        public Button upgradeForceBtn;
        public static List<WeaponUpgradeTable> UpgradeTables => SessionManager.Session.wutManager.weaponUpgradeTables;
        
        public void SelectWeaponByName(string weaponName)
        {
            switch (weaponName)
            {
                case "Pistol":
                    selectedWeapon = WeaponType.Pistol;
                    selectedTmp.text = "Upgrading: Pistol";
                    break;
                case "Launcher":
                    selectedWeapon = WeaponType.Launcher;
                    selectedTmp.text = "Upgrading: Launcher";
                    break;
            }
            
            ValidateUpgradeButtons();
        }

        public void UpgradeAmmo()
        {
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            
            var newData =  new WeaponUpgradeData()
            {
                level = weapon.ammoData.level + 1,
                upgradeType = UpgradeType.Ammo
            };
            
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoData = newData,
                damageData = weapon.damageData,
                explosionData = weapon.explosionData,
                forceData = weapon.forceData
            };
            
            UseResources(selectedWeapon, newData);

            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
        }
        
        public void UpgradeDamage()
        {
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            
            var newData =  new WeaponUpgradeData()
            {
                level = weapon.damageData.level + 1,
                upgradeType = UpgradeType.Damage
            };
            
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoData = weapon.ammoData,
                damageData = newData,
                explosionData = weapon.explosionData,
                forceData = weapon.forceData
            };
            
            UseResources(selectedWeapon, newData);

            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
        }
        
        public void UpgradeExplosion()
        {
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            
            var newData = new WeaponUpgradeData()
            {
                level = weapon.explosionData.level + 1,
                upgradeType = UpgradeType.ExplosionRadius
            };
            
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoData = weapon.ammoData,
                damageData = weapon.damageData,
                explosionData = newData,
                forceData = weapon.forceData
            };
            
            UseResources(selectedWeapon, newData);

            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
        }
        
        public void UpgradeForce()
        {
            upgradeForceBtn.interactable = false;
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            
            var newData = new WeaponUpgradeData()
            {
                level = weapon.forceData.level + 1,
                upgradeType = UpgradeType.MaxForce
            };
            
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoData = weapon.ammoData,
                damageData = weapon.damageData,
                explosionData = weapon.explosionData,
                forceData = newData
            };
            
            UseResources(selectedWeapon, newData);
            
            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
            
        }

        private void UseResources(WeaponType wpnType, WeaponUpgradeData data)
        {
            
            //TODO: Make this look up in WeaponUpgradeTableManager
            var wut = UpgradeTables.Find(x =>
                x.weaponType == wpnType && x.upgradeType == data.upgradeType && x.level == data.level);

            if (!wut)
            {
                Debug.LogWarning($"Could not find a WUT for {wpnType} {data.upgradeType} Level {data.level}");
                return;
            }
            
            foreach (var res in wut.requiredResources)
            {
                Debug.Log($"Used {res.amount} {res.resourceType} to upgrade {wpnType} {data.upgradeType} to Level {data.level}.");
                SessionManager.PlayerData.UseResource(res.resourceType, res.amount);
            }
        }
        
        

        public void ValidateUpgradeButtons()
        {
            if (selectedWeapon == WeaponType.None)
            {
                selectedTmp.text = "Select a Weapon to Upgrade";
                upgradeDamageBtn.interactable = false;
                upgradeAmmoBtn.interactable = false;
                upgradeExplosionBtn.interactable = false;
                upgradeForceBtn.interactable = false;
                return;
            }

            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            var wutManager = SessionManager.Session.wutManager;
            foreach (var type in Enum.GetValues(typeof(UpgradeType)))
            {
                var t = (UpgradeType)type;
                WeaponUpgradeTable wut;
                switch (t)
                {
                    case UpgradeType.Ammo:
                        wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.Ammo, weapon.ammoData.level + 1);
                        upgradeAmmoBtn.interactable = wut != null && wut.CanUpgrade();
                        break;
                    case UpgradeType.Damage:
                        wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.Damage, weapon.damageData.level + 1);
                        upgradeDamageBtn.interactable = wut != null && wut.CanUpgrade();
                        break;
                    case UpgradeType.MaxForce:
                        wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.MaxForce, weapon.forceData.level + 1);
                        upgradeForceBtn.interactable = wut != null && wut.CanUpgrade();
                        break;
                    case UpgradeType.ExplosionRadius:
                        wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.ExplosionRadius, weapon.explosionData.level + 1);
                        upgradeExplosionBtn.interactable = wut != null && wut.CanUpgrade();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Upgrade Type {t} not defined!");
                }
            }
        }
    }
}
