using TMPro;
using UnityEngine;

public class ItemTooltipUI : TooltipUI
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;

    public void ShowToolTip(ItemDataEquipment _item)
    {
        if (!_item)
            return;

        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDescription();

        AdjustFontSize(itemNameText);
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}