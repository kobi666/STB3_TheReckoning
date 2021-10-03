using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public static ItemManager instance;
    private PlayerTowersManager PlayerTowersManager;
    
    
    

    private GameManager GameManager;
    public Dictionary<int,LootItemBase> CurrentItems = new Dictionary<int, LootItemBase>();
    
    public Dictionary<int,TowerComponentItem> TowerComponentItems = new Dictionary<int, TowerComponentItem>();

    
    
    
    public event Action<int, LootItemBase> onLootItemGet;

    public void OnLootItemGet(int id, LootItemBase item)
    {
        if (!CurrentItems.ContainsKey(id))
        {
            CurrentItems.Add(id,item);
        }

        if (item is TowerComponentItem tci)
        {
            if (!TowerComponentItems.ContainsKey(id))
            {
                TowerComponentItems.Add(id,tci);
            }
        }        
        
    }

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
        GameManager = GetComponent<GameManager>();
        PlayerTowersManager = GetComponent<PlayerTowersManager>();
        instance = this;
    }

    
}
