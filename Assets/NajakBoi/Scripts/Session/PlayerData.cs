using System;
using System.Collections.Generic;
using System.IO;
using NajakBoi.Scripts.Systems.Economy;
using NajakBoi.Scripts.Systems.Statistics;
using NajakBoi.Scripts.Systems.Upgrading;
using NajakBoi.Scripts.Weapons;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public class PlayerData
    {
        public Dictionary<ResourceType, Resource> Resources = new();

        public Dictionary<WeaponType, WeaponData> Weapons = new();

        public PlayerStats Stats;

        public void AddAllResources()
        {
            var newResources = new Dictionary<ResourceType, Resource>();
            foreach (var res in Resources)
            {
                newResources.Add(res.Key, new Resource(){resourceType = res.Key, amount = 1000});
            }

            Resources = newResources;
            SaveResources();
        }  
        public void ClearAllResources()
        {
            Resources = new Dictionary<ResourceType, Resource>();
            PopulateResourcesDictionary();
        }

        public void SavePlayerData()
        {
            SaveResources();
            SavePlayerStats();
            SaveWeapons();
        }

        public void LoadPlayerData()
        {
            LoadResources();
            LoadPlayerStats();
            LoadWeapons();
        }

        #region PlayerStats

        public void UpdatePlayerStats(PlayerStats newStats)
        {
            Stats = newStats;
            SavePlayerStats();
        }

        // Method to serialize the PlayerStats to a JSON file
        private void SavePlayerStats()
        {
            var json = JsonUtility.ToJson(Stats);
            var filePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
            File.WriteAllText(filePath, json);
        }

        // Method to deserialize the PlayerStats from a JSON file
        private void LoadPlayerStats()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var stats = JsonUtility.FromJson<PlayerStats>(json);
                Stats = stats;
                return;
            }

            PopulatePlayerStats();
        }

        private void PopulatePlayerStats()
        {
            Stats = new PlayerStats()
            {
                Level = 1,
                Experience = 0,
                MaxHealth = 50f,
                MaxJumpHeight = 1.2f,
                MaxMovement = 100f,
                BlastResistance = 5f,
                PierceResistance = 5f,
                ColdResistance = 5f,
                FireResistance = 5f,
                LightningResistance = 5f,
                PoisonResistance = 5f
            };
            SavePlayerStats();
        }

        #endregion

        #region Resources

        // Method used to ADD resource to the player.
        public void GainResource(ResourceType type, int amount)
        {
            var resource = Resources[type];
            resource.amount += amount;
            Resources[type] = resource;

            SaveResources();
        }

        // Method used to SUBTRACT resource from the player.
        public void UseResource(ResourceType type, int amount)
        {
            var resource = Resources[type];
            resource.amount -= amount;
            Resources[type] = resource;

            SaveResources();
        }

        public void PopulateResourcesDictionary()
        {
            foreach (var type in Enum.GetValues(typeof(ResourceType)))
            {
                var t = (ResourceType)type;

                if (Resources.ContainsKey(t)) continue;

                var res = new Resource()
                {
                    resourceType = t,
                    amount = 0
                };

                Resources.Add(t, res);
            }

            SaveResources();
        }

        // Method to serialize the Resources dictionary to a JSON file
        private void SaveResources()
        {
            var json = JsonUtility.ToJson(new SerializableResourceDictionary(Resources));
            var filePath = Path.Combine(Application.persistentDataPath, "Resources.json");
            File.WriteAllText(filePath, json);
        }

        // Method to deserialize the dictionary from a JSON file
        private void LoadResources()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "Resources.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var serializableResourceDictionary = JsonUtility.FromJson<SerializableResourceDictionary>(json);
                Resources = serializableResourceDictionary.ResourcesToDictionary();
                return;
            }

            PopulateResourcesDictionary();
        }

        // Wrapper class for serialization of ResourceDictionary
        [Serializable]
        private class SerializableResourceDictionary
        {
            public List<SerializableResource> resourcesList;

            public SerializableResourceDictionary(Dictionary<ResourceType, Resource> resources)
            {
                resourcesList = new List<SerializableResource>();
                foreach (var resource in resources)
                {
                    resourcesList.Add(new SerializableResource(resource.Key, resource.Value));
                }
            }

            public Dictionary<ResourceType, Resource> ResourcesToDictionary()
            {
                var resourcesToDictionary = new Dictionary<ResourceType, Resource>();
                foreach (var serializableResource in resourcesList)
                {
                    resourcesToDictionary.Add(serializableResource.resourceType, serializableResource.resource);
                }

                return resourcesToDictionary;
            }
        }

        // Wrapper class for serialization of Resource
        [Serializable]
        private class SerializableResource
        {
            public ResourceType resourceType;
            public Resource resource;

            public SerializableResource(ResourceType type, Resource res)
            {
                resourceType = type;
                resource = res;
            }
        }

        #endregion

        #region Weapons

        public void UpgradeWeapon(WeaponType type, WeaponData data)
        {
            Weapons[type] = data;
            SaveWeapons();
        }

        public void PopulateWeaponsDictionary()
        {
            foreach (var type in Enum.GetValues(typeof(WeaponType)))
            {
                var t = (WeaponType)type;

                if (Weapons.ContainsKey(t) || t == WeaponType.None) continue;

                var wpn = new WeaponData()
                {
                    weaponType = t,
                    damageData = new WeaponUpgradeData()
                    {
                        upgradeType = UpgradeType.Damage,
                        level = 0
                    },
                    ammoData = new WeaponUpgradeData()
                    {
                        upgradeType = UpgradeType.Ammo,
                        level = 0
                    },
                    forceData = new WeaponUpgradeData()
                    {
                        upgradeType = UpgradeType.MaxForce,
                        level = 0
                    },
                    explosionData = new WeaponUpgradeData()
                    {
                        upgradeType = UpgradeType.ExplosionRadius,
                        level = 0
                    },
                };

                Weapons.Add(t, wpn);
            }

            SaveWeapons();
        }

        // Method to serialize the Resources dictionary to a JSON file
        private void SaveWeapons()
        {
            var json = JsonUtility.ToJson(new SerializableWeaponsDictionary(Weapons));
            var filePath = Path.Combine(Application.persistentDataPath, "Weapons.json");
            File.WriteAllText(filePath, json);
        }

        // Method to deserialize the dictionary from a JSON file
        private void LoadWeapons()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "Weapons.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var serializableWeaponsDictionary = JsonUtility.FromJson<SerializableWeaponsDictionary>(json);
                Weapons = serializableWeaponsDictionary.WeaponsToDictionary();
                return;
            }

            PopulateWeaponsDictionary();
        }

        // Wrapper class for serialization of ResourceDictionary
        [Serializable]
        private class SerializableWeaponsDictionary
        {
            public List<SerializableWeapon> weaponsList;

            public SerializableWeaponsDictionary(Dictionary<WeaponType, WeaponData> weapons)
            {
                weaponsList = new List<SerializableWeapon>();
                foreach (var weapon in weapons)
                {
                    weaponsList.Add(new SerializableWeapon(weapon.Key, weapon.Value));
                }
            }

            public Dictionary<WeaponType, WeaponData> WeaponsToDictionary()
            {
                var weaponsToDictionary = new Dictionary<WeaponType, WeaponData>();
                foreach (var serializableWeapon in weaponsList)
                {
                    weaponsToDictionary.Add(serializableWeapon.weaponType, serializableWeapon.weaponData);
                }

                return weaponsToDictionary;
            }
        }

        // Wrapper class for serialization of Resource
        [Serializable]
        private class SerializableWeapon
        {
            public WeaponType weaponType;
            public WeaponData weaponData;
            
            public SerializableWeapon(WeaponType type, WeaponData data)
            {
                weaponType = type;
                weaponData = data;
            }
        }

        #endregion

        //Utilities
        //Bandage
        //HealAmount
        //AvailableUses
        //MedKit
        //HealAmount
        //AdditionalRegen
        //AvailableUses
        //Ramen
        //HealAmount
        //AdditionalRegen
        //AdditionalDamage
        //AvailableUses


    }
}
