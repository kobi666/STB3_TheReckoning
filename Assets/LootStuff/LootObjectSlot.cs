using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class LootObjectSlot : MyGameObject
{
    [Required]
    public SpriteRenderer SpriteRenderer;
    
    [SerializeReference][TypeFilter("getLootItems")]
    public LootItemBase LootItem;

    private static IEnumerable<Type> getLootItems()
    {
        return LootItemBase.GetLootItems();
    }

    public LootObject LootObject = null;

    public bool SlotActive()
    {
        return (LootObject != null);
    }

    public int SlotIndex = 0;

    private void Awake()
    { 
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
