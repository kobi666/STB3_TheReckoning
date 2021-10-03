using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public abstract class LootItemBase
{
    private int _itemID = 0;
    
    [ShowInInspector]
    public int ItemID
    {
        get
        {
            if (_itemID == 0)
            {
                _itemID = ItemManager.instance?.ItemID ?? 0;
                return _itemID;
            }

            return _itemID;
        }
        
    }
    
    public static IEnumerable<Type> GetLootItems()
    {
        var q = typeof(LootItemBase).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(LootItemBase))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    public abstract void Init();
    public abstract Sprite ItemSprite { get; }

    public abstract void OnItemGetSpecific(ItemManager ItemManager);

    public void OnItemGet(ItemManager ItemManager)
    {
        try
        {
            ItemManager.CurrentItems.Add(_itemID,this);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            
        }
        
        OnItemGetSpecific(ItemManager);
    }

}


public abstract class TowerComponentItem : LootItemBase
{
    [FormerlySerializedAs("componentFilter")] public ComponentFilterChecker componentFilterChecker = new ComponentFilterChecker();
    

    public void ApplyItemToComponent(TowerComponent towerComponent)
    {
        if (componentFilterChecker.CheckIfCanBuffComponent(towerComponent))
        {
            ApplyItemToComponentInternal(towerComponent);
        }
    }

    public abstract void ApplyItemToComponentInternal(TowerComponent towerComponent);
}

[System.Serializable]
public class BuffEffectItem : TowerComponentItem
{
    
    
    [SerializeReference][TypeFilter("GetEffects")]
    public List<Effect> Buffs = new List<Effect>();

    private static IEnumerable<Type> GetEffects()
    {
        return Effect.GetEffects();
    }
    
    public int BuffID;
    public Sprite itemSprite;
    public override Sprite ItemSprite { get => itemSprite; }
    


    public override void OnItemGetSpecific(ItemManager gameManager)
    {
       
    }

    

    public override void Init()
    {
        BuffID = ItemManager.instance.BuffID;
    }

    public override void ApplyItemToComponentInternal(TowerComponent towerComponent)
    {
        foreach (var buff in Buffs)
        {
            towerComponent.UpdateEffect(buff);
        }
    }
}










[System.Serializable]
public class ComponentFilterChecker
{
    [SerializeReference]
    public List<ComponentFilter> ComponentFilters = new List<ComponentFilter>();

    private static IEnumerable<Type> getComponentFilters()
    {
        var q = typeof(ComponentFilter).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(ComponentFilter))); // Excludes classes not inheriting from BaseClass
        
        return q;
    }

    public IEnumerable<TowerComponent> CheckIfCanBuffComponents(IEnumerable<TowerComponent> components)
    {
        List<TowerComponent> _components = new List<TowerComponent>();
        foreach (var component in components)
        {
            foreach (var filter in ComponentFilters)
            {
                if (filter.ValidateComponent(component))
                {
                    _components.Add(component);
                    break;
                }
            }
        }
        return _components;
    }

    public bool CheckIfCanBuffComponent(TowerComponent towerComponent)
    {
        foreach (var filter in ComponentFilters)
        {
            if (filter.ValidateComponent(towerComponent))
            {
                return true;
            }
        }

        return false;
    }

}






