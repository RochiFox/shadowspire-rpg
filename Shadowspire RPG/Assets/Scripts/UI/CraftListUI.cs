using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipment;

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    private void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        foreach (ItemDataEquipment equip in craftEquipment)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<CraftSlotUI>().SetupCraftSlot(equip);
        }
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        SetupCraftList();
    }

    private void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0])
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }
}