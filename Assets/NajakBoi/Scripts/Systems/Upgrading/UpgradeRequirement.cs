using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.Weapons;

namespace NajakBoi.Scripts.Systems.Upgrading
{
    public struct UpgradeRequirement
    {
        public WeaponType weaponType;
        public UpgradeType upgradeType;
        public int level;
        public int requiredPlayerLevel;
        public Resource[] requiredResources;
    }
}
