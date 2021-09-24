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

    public abstract void OnItemGetSpecific(GameManager gameManager);

    public void OnItemGet(GameManager gameManager)
    {
        try
        {
            gameManager.ItemManager.CurrentItems.Add(_itemID,this);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            
        }
        
        OnItemGetSpecific(gameManager);
    }

}

[System.Serializable]
public class BuffEffectItem : LootItemBase
{
    public ComponentFilter componentFilter = new ComponentFilter();
    
    [SerializeReference][TypeFilter("GetEffects")]
    public List<Effect> Buffs = new List<Effect>();

    private static IEnumerable<Type> GetEffects()
    {
        return Effect.GetEffects();
    }
    
    public int BuffID;
    public Sprite itemSprite;
    public override Sprite ItemSprite { get => itemSprite; }
    


    public override void OnItemGetSpecific(GameManager gameManager)
    {
        List<TowerComponent> foundComponents = new List<TowerComponent>();
        foreach (var activeTower in gameManager.PlayerTowersManager.ActiveTowers)
        {
            foreach (var towerComponent in activeTower.Value.TowerComponents)
            {
                foundComponents.Add(towerComponent);
            }
        }
        var componentsToBuff = componentFilter.CheckIfCanBuffComponents(foundComponents);
        foreach (var component in componentsToBuff)
        {
            var eflist = component.GetEffectList();
            foreach (var buff in Buffs)
            {
               component.UpdateEffect(buff, eflist); 
            }
        }
    }

    public override void Init()
    {
        BuffID = ItemManager.instance.BuffID;
    }
}










[System.Serializable]
public class ComponentFilter
{
    public List<ComponentFamily> ComponentFamilies = new List<ComponentFamily>();

    public IEnumerable<TowerComponent> CheckIfCanBuffComponents(IEnumerable<TowerComponent> components)
    {
        List<TowerComponent> _components = new List<TowerComponent>();
        foreach (var component in components)
        {
            if (ComponentFamilies.Contains(ComponentFamily.All))
            {
                _components.Add(component);
                continue;
            }
            
            if (ComponentFamilies.Contains(component.componentFamily))
            {
                _components.Add(component);
            }
        }
        return _components;
    }

}



