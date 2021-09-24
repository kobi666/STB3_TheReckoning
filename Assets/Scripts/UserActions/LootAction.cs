using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public abstract class LootAction : CursorActionBase<Object>
{
    public override void ExecAction(ButtonDirectionsNames buttonDirectionsNames)
    {
        Action(buttonDirectionsNames);
    }

    public override void InitAction(Object p, int actionIndex)
    {
        InitActionInternal();
    }

    public abstract void InitActionInternal();
    
}

[System.Serializable]
public class GetLootItem : LootAction
{
    [SerializeReference][TypeFilter("getItems")]
    public LootItemBase Item;

    private IEnumerable<Type> getItems()
    {
        return LootItemBase.GetLootItems();
    }
    public override void Action(ButtonDirectionsNames buttonDirectionsName)
    {
        Item.OnItemGet(GameManager.Instance);
    }

    public override bool ExecutionConditions()
    {
        return true;
    }

    public override int ActionCost { get; set; } = 0;
    public override Sprite ActionSprite
    {
        get => Item.ItemSprite;
        set { }
    }

    public override void InitActionInternal()
    {
        Item.Init();
    }
}

[System.Serializable]
public class AddOrReplaceTower : LootAction
{
    [Required]
    public TowerController TowerControllerPrefab;
    public override void Action(ButtonDirectionsNames buttonDirectionsName)
    {
        if (TowerControllerPrefab != null) {
        var ptm = GameManager.Instance.PlayerTowersManager;
        
            ptm.SetTowerByButtonName(buttonDirectionsName,TowerControllerPrefab);
        
        }
        else
        {
            Debug.LogWarning("NULL tower prefab");
        }
        
    }

    public override bool ExecutionConditions()
    {
        return true;
    }

    public override int ActionCost { get; set; }
    public override Sprite ActionSprite { 
        get => TowerControllerPrefab?.TowerSprite;
        set { }
    }
    public override void InitActionInternal()
    {
        
    }
}


 



