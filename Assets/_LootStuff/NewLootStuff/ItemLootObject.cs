using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemLootObject : NewLootObject
{
    [SerializeReference][TypeFilter("GetItems")]
    public LootAction LootAction;
    
    private IEnumerable<Type> GetItems()
    {
        var q = typeof(LootAction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(LootAction)));
        
        return q;
    }
    
    
    public override Sprite ObjectSprite { get => LootAction.ActionSprite(ButtonDirectionsNames.North); }
    public override void OnLootObjectSelect(LootSelector lootSelector)
    {
        LootAction.ExecAction(ButtonDirectionsNames.North);
    }
}
