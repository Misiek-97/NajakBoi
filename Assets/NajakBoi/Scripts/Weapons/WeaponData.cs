using NajakBoi.Scripts.Systems.Upgrading;

namespace NajakBoi.Scripts.Weapons
{
    [System.Serializable]
    public struct WeaponData
    {
        public WeaponType weaponType;
        public WeaponUpgradeData damageData;
        public WeaponUpgradeData ammoData;
        public WeaponUpgradeData forceData;
        public WeaponUpgradeData explosionData;
    }
}
