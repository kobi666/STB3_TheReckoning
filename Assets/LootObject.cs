using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class LootObject : MonoBehaviour
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

}
