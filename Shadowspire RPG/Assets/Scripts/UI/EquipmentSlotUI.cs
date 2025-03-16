using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType slotType;

    void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }
}
