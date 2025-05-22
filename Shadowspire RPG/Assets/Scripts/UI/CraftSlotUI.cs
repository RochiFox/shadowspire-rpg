using UnityEngine.EventSystems;

public class CraftSlotUI : ItemSlotUI
{
    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if (!_data)
            return;

        item.data = _data;
        itemImage.sprite = _data.itemIcon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 12)
            itemText.fontSize *= .7f;
        else
            itemText.fontSize = 24;
    }

    public override void OnPointerDown(PointerEventData _eventData)
    {
        UI.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);
    }
}