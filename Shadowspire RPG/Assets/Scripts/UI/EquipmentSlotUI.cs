using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData _eventData)
    {
        if (item == null || !item.data)
            return;

        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);

        UI.itemToolTip.HideToolTip();

        CleanUpSlot();
    }
}