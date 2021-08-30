using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public abstract class LootItem : MonoBehaviour
{
    public abstract Sprite ItemSprite { get; }
    
    public abstract void OnItemGet();

    public abstract void Init();
}


[System.Serializable]
public class BuffItem : LootItem
{
    public int BuffID;
    public Sprite itemSprite;
    public override Sprite ItemSprite { get => itemSprite; }
    public override void OnItemGet()
    {
        
    }

    public override void Init()
    {
        BuffID = ItemManager.instance.BuffID;
    }
    
    [FormerlySerializedAs("BuffFilter")] public ComponentFilter componentFilter = new ComponentFilter();
}

[System.Serializable]
public abstract class BuffFunction
{
    
}


[System.Serializable]
public class BuffEffect : BuffFunction
{
    public List<Effect> EffectBuffs = new List<Effect>();
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
            if (ComponentFamilies.Contains(component.componentFamily))
            {
                _components.Add(component);
            }
        }
        return _components;
    }

}
