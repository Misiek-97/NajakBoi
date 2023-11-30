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
 
        public void SelectWeaponByName(string weaponName)
        {
            switch (weaponName)
            {
                case "Pistol":
                    selectedWeapon = WeaponType.Pistol;
                    upgradeDamageBtn.interactable = true;
                    upgradeAmmoBtn.interactable = false;
                    upgradeExplosionBtn.interactable = false;
                    upgradeForceBtn.interactable = false;
                    selectedTmp.text = "Upgrading: Pistol";
                    break;
                case "Launcher":
                    selectedWeapon = WeaponType.Launcher;
                    upgradeDamageBtn.interactable = true;
                    upgradeAmmoBtn.interactable = true;
                    upgradeExplosionBtn.interactable = true;
                    upgradeForceBtn.interactable = true;
                    selectedTmp.text = "Upgrading: Launcher";
                    break;
            }
            
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
            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
            
        }
        
        public void UpgradeForce()
        {
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
            SessionManager.PlayerData.UpgradeWeapon(selectedWeapon, weaponData);
            LabManager.Instance.DisplayPanelByName("Upgrade");
            
        }
    }
}
