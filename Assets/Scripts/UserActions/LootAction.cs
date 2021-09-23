using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public abstract class LootAction : CursorActionBase<Object>
{
    public override void ExecAction()
    {
        Action();
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
    public override void Action()
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
    public override void Action()
    {
        throw new NotImplementedException();
    }

    public override bool ExecutionConditions()
    {
        throw new NotImplementedException();
    }

    public override int ActionCost { get; set; }
    public override Sprite ActionSprite { get; set; }
    public override void InitActionInternal()
    {
        throw new NotImplementedException();
    }
}


 



