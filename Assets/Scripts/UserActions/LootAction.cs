using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public abstract class LootAction : CursorActionBase<LootObject>
{

    public abstract LootActions LootActions();
    public override void ExecAction(ButtonDirectionsNames buttonDirectionsNames)
    {
        Action(buttonDirectionsNames);
    }

    public override void InitAction(LootObject p, int actionIndex)
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
        Item.OnItemGet(ItemManager.instance);
    }

    public override bool ExecutionConditions()
    {
        return true;
    }

    public override int ActionCost { get; set; } = 0;

    public override Sprite ActionSprite(ButtonDirectionsNames buttonDirectionsName)
    {
        return Item.ItemSprite;
    }

    private LootItemActions LootItemActions = new LootItemActions();

    public override LootActions LootActions()
    {
        return LootItemActions;
    }

    public override void InitActionInternal()
    {
        Item.Init();
    }
}




 



