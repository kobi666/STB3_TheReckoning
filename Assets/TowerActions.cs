using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TowerActions : CursorActions<TowerAction,TowerSlotController>
{
    public override IEnumerable<Type> GetActions()
    {
        {
            var q = typeof(TowerAction).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(TowerAction)));
        
            return q;
        }
    }
}
