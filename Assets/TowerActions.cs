using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TowerActions : CursorActions<TowerAction,TowerSlotController>
{
    public override TowerAction Action_North { get => ActionsByIndex[0]; set => ActionsByIndex[0] = value ; }
    public override TowerAction Action_East { get => ActionsByIndex[1]; set => ActionsByIndex[1] = value ; }
    public override TowerAction Action_South { get => ActionsByIndex[2]; set => ActionsByIndex[2] = value ; }
    public override TowerAction Action_West { get => ActionsByIndex[3]; set => ActionsByIndex[3] = value ; }

    public override IEnumerable<Type> GetActions()
    {
        
            var q = typeof(TowerAction).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(TowerAction)));
        
            return q;
        
    }
}
