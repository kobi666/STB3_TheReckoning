using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public static ItemManager instance;
    
    public Dictionary<int,LootItemBase> CurrentItems = new Dictionary<int, LootItemBase>();

    private int _buffID = 0;

    public int BuffID
    {
        get
        {
            _buffID++;
            return _buffID;
        }
    }
    
    private int _itemID = 0;

    public int ItemID
    {
        get
        {
            _itemID++;
            return _itemID;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    
}
