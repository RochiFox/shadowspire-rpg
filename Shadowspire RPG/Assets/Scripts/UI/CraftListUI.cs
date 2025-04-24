using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipment;

    void Start()
    {
        transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<CraftSlotUI>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }
}
