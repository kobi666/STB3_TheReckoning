using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;

public class LootItemManager : MonoBehaviour
{
    public static LootItemManager instance;
    
    
    [Required]
    public NewLootSlot Slot_Left;
    [Required]
    public NewLootSlot Slot_Center;
    [Required]
    public NewLootSlot Slot_Right;
    
    public List<NewLootSlot> AllSlots = new List<NewLootSlot>();
    
    

    public void Start()
    {
        instance = this;
        AllSlots.Add(Slot_Center);
        AllSlots.Add(Slot_Left);
        AllSlots.Add(Slot_Right);
    }
}
