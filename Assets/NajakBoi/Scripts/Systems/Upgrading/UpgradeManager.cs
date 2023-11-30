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
            var newAmmoLevel = weapon.ammoLevel + 1;
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoLevel = newAmmoLevel,
                damageLevel = weapon.damageLevel,
                explosionLevel = weapon.explosionLevel,
                forceLevel = weapon.forceLevel
            };
            
            UseResources(newAmmoLevel, selectedWeapon, UpgradeType.Ammo);

            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
        }
        
        public void UpgradeDamage()
        {
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            var newDamageLevel = weapon.damageLevel + 1;
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoLevel = weapon.ammoLevel,
                damageLevel = newDamageLevel,
                explosionLevel = weapon.explosionLevel,
                forceLevel = weapon.forceLevel
            };
            
            UseResources(newDamageLevel, selectedWeapon, UpgradeType.Damage);

            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
        }
        
        public void UpgradeExplosion()
        {
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            var newExplosionLevel = weapon.explosionLevel + 1;
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoLevel = weapon.ammoLevel,
                damageLevel = weapon.damageLevel,
                explosionLevel = newExplosionLevel,
                forceLevel = weapon.forceLevel
            };
            
            UseResources(newExplosionLevel, selectedWeapon, UpgradeType.ExplosionRadius);

            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
        }
        
        public void UpgradeForce()
        {
            upgradeForceBtn.interactable = false;
            var weapon = SessionManager.PlayerData.Weapons[selectedWeapon];
            var newForceLevel = weapon.forceLevel + 1;
            var weaponData = new WeaponData()
            {
                weaponType = selectedWeapon,
                ammoLevel = weapon.ammoLevel,
                damageLevel = weapon.damageLevel,
                explosionLevel = weapon.explosionLevel,
                forceLevel = newForceLevel
            };
            
            UseResources(newForceLevel, selectedWeapon, UpgradeType.MaxForce);
            
            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
            
        }

        private void UseResources(int level, WeaponType wpnType, UpgradeType upType)
        {
            var wut = UpgradeTables.Find(x =>
                x.weaponType == wpnType && x.upgradeType == upType && x.level == level);

            foreach (var res in wut.requiredResources)
            {
                Debug.Log($"Used {res.amount} {res.resourceType} to upgrade {wpnType} {upType} to Level {level}.");
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
            
            foreach (var wut in UpgradeTables.Where(x => x.weaponType == selectedWeapon))
            {
                if (wut.upgradeType == UpgradeType.Ammo && wut.level == weapon.ammoLevel + 1)
                {
                    upgradeAmmoBtn.interactable = wut.CanUpgrade();
                }
                
                if (wut.upgradeType == UpgradeType.Damage && wut.level == weapon.damageLevel + 1)
                {
                    upgradeDamageBtn.interactable = wut.CanUpgrade();
                }
            }
            
            
            upgradeExplosionBtn.interactable = false;
            upgradeForceBtn.interactable = false;
            
        }
    }
}
