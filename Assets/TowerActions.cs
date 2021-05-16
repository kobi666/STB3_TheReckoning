using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TowerActions
{
    private TowerSlotController parentTowerSlotController;
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction North;
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction East;
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction South;
    [TypeFilter("GetTowerActions")][SerializeReference]
    public TowerAction West;

    public TowerAction[] Actions
    {
        get
        {
            return new TowerAction[]
            {
                North,
                East,
                South,
                West
            };
        }
    }
    
    [Button]
    void north()
    {
        North.ExecAction();
    }
    [Button]
    void east()
    {
        East.ExecAction();
    }
    [Button]
    void south()
    {
        South.ExecAction();
    }
    
    [Button]
    void west()
    {
        West.ExecAction();
    }

    public void initActions(TowerSlotController tsc)
    {
        parentTowerSlotController = tsc;
        North?.InitAction(tsc);
        East?.InitAction(tsc);
        South?.InitAction(tsc);
        West?.InitAction(tsc);
    }
    
    private static IEnumerable<Type> GetTowerActions()
    {
        var q = typeof(TowerAction).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(TowerAction)));
        
        return q;
    }
}
