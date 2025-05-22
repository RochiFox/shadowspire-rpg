using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")] [SerializeField]
    private float chanceToLooseItems;

    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        foreach (InventoryItem item in itemsToUnequip)
        {
            inventory.UnequipItem(item.data as ItemDataEquipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
        }

        foreach (InventoryItem material in materialsToLoose)
        {
            inventory.RemoveItem(material.data);
        }
    }
}