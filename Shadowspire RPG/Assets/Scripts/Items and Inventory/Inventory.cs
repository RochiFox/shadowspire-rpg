using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    [SerializeField] private Player player;

    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    private Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    private Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    private Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")] [SerializeField]
    private Transform inventorySlotParent;

    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private ItemSlotUI[] inventoryItemSlot;
    private ItemSlotUI[] stashItemSlot;
    private EquipmentSlotUI[] equipmentSlot;
    private StatSlotUI[] statSlot;

    [Header("Items cooldown")] private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Data base")] public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
        statSlot = statSlotParent.GetComponentsInChildren<StatSlotUI>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        if (loadedItems.Count > 0 || loadedEquipment.Count > 0)
        {
            foreach (ItemDataEquipment item in loadedEquipment)
            {
                EquipItem(item);
            }

            if (loadedItems.Count > 0)
            {
                foreach (InventoryItem item in loadedItems)
                {
                    for (int i = 0; i < item.stackSize; i++)
                    {
                        AddItem(item.data);
                    }
                }

                return;
            }
        }

        foreach (ItemData item in startingItems.Where(_item => _item))
        {
            AddItem(item);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary.Where(_valuePair =>
                     newEquipment && _valuePair.Key.equipmentType == newEquipment.equipmentType))
        {
            oldEquipment = item.Key;
        }

        if (oldEquipment)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);

        if (newEquipment)
        {
            equipmentDictionary.Add(newEquipment, newItem);
            newEquipment.AddModifiers();
        }

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemDataEquipment _itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(_itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
            _itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        foreach (EquipmentSlotUI slot in equipmentSlot)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == slot.slotType)
                    slot.UpdateSlot(item.Value);
            }
        }

        foreach (ItemSlotUI itemSlot in inventoryItemSlot)
        {
            itemSlot.CleanUpSlot();
        }

        foreach (ItemSlotUI itemSlot in stashItemSlot)
        {
            itemSlot.CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        foreach (StatSlotUI slot in statSlot)
        {
            slot.UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        switch (_item.itemType)
        {
            case ItemType.Equipment when CanAddItem():
                AddToInventory(_item);
                break;
            case ItemType.Material:
                AddToStash(_item);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        return inventory.Count < inventoryItemSlot.Length;
    }

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        foreach (InventoryItem requiredItem in _requiredMaterials)
        {
            if (stashDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if (stashItem.stackSize < requiredItem.stackSize)
                {
                    Debug.Log("Not enough materials: " + requiredItem.data.name);
                }
                else
                {
                    Debug.Log("Materials not found in stash: " + requiredItem.data.name);
                }

                return false;
            }
        }

        foreach (InventoryItem requiredMaterial in _requiredMaterials)
        {
            for (int i = 0; i < requiredMaterial.stackSize; i++)
            {
                RemoveItem(requiredMaterial.data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("Craft is successful: " + _itemToCraft.name);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipment(EquipmentType _type)
    {
        ItemDataEquipment equippedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary.Where(_item =>
                     _item.Key.equipmentType == _type))
        {
            equippedItem = item.Key;
        }

        return equippedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (!currentFlask)
        {
            player.fx.CreatePopUpText("You don't have flask");
            return;
        }

        if (player.stats.currentHealth >= player.stats.maxHealth.GetValue())
        {
            player.fx.CreatePopUpText("You are already at full health");
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;

            InGameUI.instance.StartFlaskCooldown(flaskCooldown);

            UnequipItem(currentFlask);

            foreach (EquipmentSlotUI slot in equipmentSlot)
            {
                if (slot.slotType == EquipmentType.Flask)
                {
                    slot.CleanUpSlot();
                }
            }

            UpdateSlotUI();
        }
        else
            player.fx.CreatePopUpText("Flask on cooldown;");
    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on cooldown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (ItemData item in itemDataBase)
            {
                if (item && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item)
                    {
                        stackSize = pair.Value
                    };

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (ItemData item in from loadedItemId in _data.equipmentId
                 from item in itemDataBase
                 where item && loadedItemId == item.itemId
                 select item)
        {
            loadedEquipment.Add(item as ItemDataEquipment);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    private static List<ItemData> GetItemDataBase()
    {
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        return assetNames.Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemData>).ToList();
    }
#endif
}