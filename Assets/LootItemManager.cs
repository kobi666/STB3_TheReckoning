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
    public LootObjectSlot Slot_0;
    [Required]
    public LootObjectSlot Slot_1;
    [Required]
    public LootObjectSlot Slot_2;
    [Required]
    public LootObjectSlot Slot_3;
    
    private IEnumerable<Type> GetLootItems()
    {
        return LootItemBase.GetLootItems();
    }
    
    public Dictionary<int,LootObjectSlot> LootObjectSlotsByIndex = new Dictionary<int, LootObjectSlot>();

    public Dictionary<int, LootObjectSlot> AvailableLootSlotsByIndex
    {
        get
        {
            if (LootObjectSlotsByIndex.IsNullOrEmpty())
            {
                return new Dictionary<int, LootObjectSlot>();
            }
            Dictionary<int, LootObjectSlot> dict = new Dictionary<int, LootObjectSlot>();
            foreach (var lootObjectSlot in LootObjectSlotsByIndex)
            {
                if (lootObjectSlot.Value.SlotActive())
                {
                    dict.Add(lootObjectSlot.Key,lootObjectSlot.Value);
                }
            }

            return dict; 
        }
    }

    

    public (LootObjectSlot, LootObjectSlot) AvailableSlotsLeftRight(int currentSlotIndex)
    {
        var avilableslots = AvailableLootSlotsByIndex;
        (LootObjectSlot, LootObjectSlot) slots = (null, null);
        if (avilableslots.ContainsKey(currentSlotIndex - 1))
        {
            slots.Item1 = avilableslots[currentSlotIndex - 1];
        }

        if (avilableslots.ContainsKey(currentSlotIndex + 1))
        {
            slots.Item2 = avilableslots[currentSlotIndex + 1];
        }
        return slots;
    }

    private void Awake()
    {
        instance = this;
        /*Slot_0.SlotIndex = 0;
        Slot_1.SlotIndex = 1;
        Slot_2.SlotIndex = 2;
        Slot_3.SlotIndex = 3;
        LootObjectSlotsByIndex.Add(Slot_0.SlotIndex,Slot_0);
        LootObjectSlotsByIndex.Add(Slot_1.SlotIndex,Slot_1);
        LootObjectSlotsByIndex.Add(Slot_2.SlotIndex,Slot_2);
        LootObjectSlotsByIndex.Add(Slot_3.SlotIndex,Slot_3);*/
    }
}
