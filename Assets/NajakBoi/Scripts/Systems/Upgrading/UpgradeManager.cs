using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public TextMeshProUGUI weaponInfoTmp;
        public TextMeshProUGUI requirementsInfoTmp;
        public WeaponType selectedWeapon;
        public UpgradeType selectedUpgrade;

        public Button upgradeBtn;
        public Button upgradeAmmoBtn;
        public Button upgradeDamageBtn;
        public Button upgradeForceBtn;
        public Button upgradeExplosionBtn;
        
        public Button upgradeBuildXBtn;
        public Button upgradeBuildYBtn;
        public Button upgradeMaxWeightBtn;
        public static List<WeaponUpgradeTable> UpgradeTables => SessionManager.Session.wutManager.weaponUpgradeTables;


        private void Awake()
        {
            ValidateUpgradeButton();
            ValidateUpgradeBuildButtons();
        }

        public void UpgradeBuildX()
        {
            SessionManager.PlayerData.BuildingStats.maxBuildX++;
            ValidateUpgradeBuildButtons();

        } 
        
        public void UpgradeBuildY()
        {
            SessionManager.PlayerData.BuildingStats.maxBuildY++;
            ValidateUpgradeBuildButtons();
        }
        
        public void UpgradeWeight()
        {
            SessionManager.PlayerData.BuildingStats.maxWeight += 10;
            ValidateUpgradeBuildButtons();
        }

        private void ValidateUpgradeBuildButtons()
        {
            upgradeBuildXBtn.interactable = SessionManager.PlayerData.BuildingStats.maxBuildX < SessionManager.MaxBuildX;
            upgradeBuildYBtn.interactable = SessionManager.PlayerData.BuildingStats.maxBuildY < SessionManager.MaxBuildY;
            upgradeMaxWeightBtn.interactable = SessionManager.PlayerData.BuildingStats.maxWeight < SessionManager.MaxBuildWeight;
        }

        public void SelectWeaponByName(string weaponName)
        {
            selectedWeapon = (WeaponType)Enum.Parse(typeof(WeaponType), weaponName);
            var wpn = SessionManager.PlayerData.Weapons[selectedWeapon];
            var sb = new StringBuilder();
            sb.AppendLine($"<b>{weaponName}</b>");
            sb.AppendLine($"\r\nAmmo Level: {wpn.ammoData.level}");
            sb.AppendLine($"\r\nDamage Level: {wpn.damageData.level}");
            sb.AppendLine($"\r\nForce Level: {wpn.forceData.level}");
            sb.AppendLine($"\r\nExplosion Level: {wpn.explosionData.level}");
            weaponInfoTmp.text = sb.ToString();
            
            foreach (var type in Enum.GetValues(typeof(UpgradeType)))
            {
                var upType = (UpgradeType)type;
                switch (upType)
                {
                    case UpgradeType.None:
                        break;
                    case UpgradeType.Ammo:
                        upgradeAmmoBtn.interactable =
                                SessionManager.Session.wutManager.GetWutFor(selectedWeapon, upType,
                                    wpn.ammoData.level + 1) != null;
                        break;
                    case UpgradeType.Damage:
                        upgradeDamageBtn.interactable =
                            SessionManager.Session.wutManager.GetWutFor(selectedWeapon, upType,
                                wpn.damageData.level + 1) != null;
                        break;
                    case UpgradeType.MaxForce:
                        upgradeForceBtn.interactable =
                            SessionManager.Session.wutManager.GetWutFor(selectedWeapon, upType,
                                wpn.forceData.level + 1) != null;
                        break;
                    case UpgradeType.ExplosionRadius:
                        upgradeExplosionBtn.interactable =
                            SessionManager.Session.wutManager.GetWutFor(selectedWeapon, upType,
                                wpn.explosionData.level + 1) != null;
                        break;
                    case UpgradeType.MaxBuildWeight:
                    case UpgradeType.MaxBuildY:
                    case UpgradeType.MaxBuildX:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Upgrade type {upType} not defined!");
                }
            }
            
        }

        public void SelectUpgradeByName(string upgradeName)
        {
            if (selectedWeapon == WeaponType.None)
            {
                upgradeBtn.interactable = false;
                weaponInfoTmp.text = "Select a Weapon to Upgrade!";
                return;
            }
            
            selectedUpgrade = (UpgradeType)Enum.Parse(typeof(UpgradeType), upgradeName);
            var wpn = SessionManager.PlayerData.Weapons[selectedWeapon];
            WeaponUpgradeTable wut = selectedUpgrade switch
            {
                UpgradeType.Ammo => SessionManager.Session.wutManager.GetWutFor(selectedWeapon, selectedUpgrade,
                    wpn.ammoData.level + 1),
                UpgradeType.Damage => SessionManager.Session.wutManager.GetWutFor(selectedWeapon, selectedUpgrade,
                    wpn.damageData.level + 1),
                UpgradeType.MaxForce => SessionManager.Session.wutManager.GetWutFor(selectedWeapon, selectedUpgrade,
                    wpn.forceData.level + 1),
                UpgradeType.ExplosionRadius => SessionManager.Session.wutManager.GetWutFor(selectedWeapon,
                    selectedUpgrade, wpn.explosionData.level + 1),
                _ => throw new ArgumentOutOfRangeException($"Upgrade of type {selectedUpgrade} is not set up!")
            };

            if (!wut)
            {
                switch (selectedUpgrade)
                {
                    case UpgradeType.None:
                        upgradeAmmoBtn.interactable = false;
                        upgradeDamageBtn.interactable = false;
                        upgradeForceBtn.interactable = false;
                        upgradeExplosionBtn.interactable = false;
                        break;
                    case UpgradeType.Ammo:
                        upgradeAmmoBtn.interactable = false;
                        break;
                    case UpgradeType.Damage:
                        upgradeDamageBtn.interactable = false;
                        break;
                    case UpgradeType.MaxForce:
                        upgradeForceBtn.interactable = false;
                        break;
                    case UpgradeType.ExplosionRadius:
                        upgradeExplosionBtn.interactable = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                upgradeBtn.interactable = false;
                requirementsInfoTmp.text = "Upgrade Not Available";
                return;
            }
            
            var resources = SessionManager.PlayerData.Resources;
            var sb = new StringBuilder();
            sb.AppendLine($"<b>Upgrade {selectedUpgrade} to Level {wut.level} Requirements</b>\r\n");
            sb.AppendLine($"Player Level: {wut.requiredPlayerLevel}\r\n");
            foreach (var res in wut.requiredResources)
            {
                sb.AppendLine($"{res.resourceType}: {res.amount} / {resources[res.resourceType].amount}\r\n");
            }

            requirementsInfoTmp.text = sb.ToString();
            
            ValidateUpgradeButton();
        }
        
        

        public void UpgradeClicked()
        {
            upgradeBtn.interactable = false;
            switch (selectedUpgrade)
            {
                case UpgradeType.None:
                    break;
                case UpgradeType.Ammo:
                    UpgradeAmmo();
                    break;
                case UpgradeType.Damage:
                    UpgradeDamage();
                    break;
                case UpgradeType.MaxForce:
                    UpgradeForce();
                    break;
                case UpgradeType.ExplosionRadius:
                    UpgradeExplosion();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Upgrade for {selectedUpgrade} is not set up!");
            }
            SelectUpgradeByName(selectedUpgrade.ToString());
        }

        private void UpdateUpgradeStatus()
        {
            SelectUpgradeByName(selectedUpgrade.ToString());
            SelectWeaponByName(selectedWeapon.ToString());
            ValidateUpgradeButton();
        }

        private void UpgradeAmmo()
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
            UpdateUpgradeStatus();
        }

        private void UpgradeDamage()
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
            UpdateUpgradeStatus();
        }

        private void UpgradeExplosion()
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
            UpdateUpgradeStatus();
        }

        private void UpgradeForce()
        {
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
            UpdateUpgradeStatus();
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



        public void ValidateUpgradeButton()
        {
            if (selectedWeapon == WeaponType.None)
            {
                weaponInfoTmp.text = "Select a Weapon to view stats.";
                upgradeBtn.interactable = false;
                return;
            }

            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            var wutManager = SessionManager.Session.wutManager;
            WeaponUpgradeTable wut;
            switch (selectedUpgrade)
            {
                case UpgradeType.None:
                    upgradeBtn.interactable = false;
                    break;
                case UpgradeType.Ammo:
                    wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.Ammo, weapon.ammoData.level + 1);
                    upgradeBtn.interactable = wut != null && wut.CanUpgrade();
                    break;
                case UpgradeType.Damage:
                    wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.Damage, weapon.damageData.level + 1);
                    upgradeBtn.interactable = wut != null && wut.CanUpgrade();
                    break;
                case UpgradeType.MaxForce:
                    wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.MaxForce, weapon.forceData.level + 1);
                    upgradeBtn.interactable = wut != null && wut.CanUpgrade();
                    break;
                case UpgradeType.ExplosionRadius:
                    wut = wutManager.GetWutFor(selectedWeapon, UpgradeType.ExplosionRadius,
                        weapon.explosionData.level + 1);
                    upgradeBtn.interactable = wut != null && wut.CanUpgrade();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Upgrade Type {selectedUpgrade} not defined!");
            }

        }
    }
}
