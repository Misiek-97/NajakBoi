using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NajakBoi.Scripts.Systems.Economy
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Custom/Create New Loot Table")]

    public class LootTable : ScriptableObject
    {
        public List<LootItem> lootItemsList = new();

        [ContextMenu("Populate Enum Values")]
        public void PopulateLootTable()
        {
            lootItemsList.Clear();
            
            foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                var lootItem = new LootItem
                {
                    name = resourceType.ToString(),
                    type = resourceType,
                    minAmount = 1,
                    maxAmount = 3,
                    dropChance = 0.0f // Set your default drop chance here
                };

                lootItemsList.Add(lootItem);
            }
        }
        
        public List<Drop> GenerateLoot()
        {
            var droppedItems = new List<Drop>();

            foreach (var lootItem in lootItemsList)
            {
                var randomValue = Random.value; // Random value between 0 and 1

                if (!(randomValue <= lootItem.dropChance)) continue;
                
                var drop = new Drop()
                {
                    Amount = Random.Range(lootItem.minAmount, lootItem.maxAmount + 1),
                    Type = lootItem.type
                };
                droppedItems.Add(drop);
                Debug.Log($"Adding Drop Item: {drop.Amount} {drop.Type}");
            }

            return droppedItems;
        }
    }

    public class Drop
    {
        public ResourceType Type;
        public int Amount;
    }
    
    
    [Serializable]
    public class LootItem
    {
        public string name;
        public ResourceType type;
        public int minAmount;
        public int maxAmount;
        [Range (0f,1f)]
        public float dropChance;
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(LootTable))]
    public class LootTableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LootTable lootTable = (LootTable)target;

            if (GUILayout.Button("Populate Loot Table"))
            {
                lootTable.PopulateLootTable();
            }
        }
    }
#endif
}
