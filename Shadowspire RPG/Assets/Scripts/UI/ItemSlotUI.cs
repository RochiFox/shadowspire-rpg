using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI UI;
    public InventoryItem item;

    protected virtual void Start()
    {
        UI = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;

            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData _eventData)
    {
        if (item == null)
            return;

        UI.itemToolTip.HideToolTip();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        if (item == null)
            return;

        UI.itemToolTip.ShowToolTip(item.data as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        if (item == null)
            return;

        UI.itemToolTip.HideToolTip();
    }
}