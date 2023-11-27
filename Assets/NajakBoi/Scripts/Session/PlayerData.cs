using System;
using System.Collections.Generic;
using System.IO;
using NajakBoi.Scripts.Systems.Economy;
using UnityEngine;

namespace NajakBoi.Scripts.Session
{
    public class PlayerData
    {
        public Dictionary<ResourceType, Resource> Resources = new ();

        public int Level;
        public int Experience;

        public void PopulateResourcesDictionary()
        {
            foreach(var type in Enum.GetValues(typeof(ResourceType)))
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

        public void SavePlayerData()
        {
            SaveResources();
        }
        
        public void LoadPlayerData()
        {
            LoadResources();
        }


        public void GainResource(ResourceType type, int amount)
        {
            var resource = Resources[type];
            resource.amount += amount;
            Resources[type] = resource;
            
            SaveResources();
        }
        public void UseResource(ResourceType type, int amount)
        {
            var resource = Resources[type];
            resource.amount -= amount;
            Resources[type] = resource;
            
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
        [System.Serializable]
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
        [System.Serializable]
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
        
        //Level
            //Experience
            //UpgradePoints
           
        //PlayerUpgrades
            //MaxHealth
            //MaxMovement
            //JumpHeight
        
        //Resistances
            //Blast
            //Pierce
            //Fire
            //Cold
            //Lightning
            //Poison

        //Weapons
            //Pistol
                //Damage
                //Rounds
                //Range
            //Launcher
                //Damage
                //MaxChargeForce
                //ExplosionRadius

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
            
                
        public void SaveToFile(string fn)
        {
            
        }

        public static PlayerData LoadFromFile(string fn)
        {
            return new PlayerData();
        }
    }
}
