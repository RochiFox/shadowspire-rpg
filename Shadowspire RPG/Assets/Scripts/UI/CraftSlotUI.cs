using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = item.data as ItemDataEquipment;

        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
