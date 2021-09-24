using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[System.Serializable]
public abstract class CursorActions<T,ParentT> where T : CursorActionBase<ParentT>
{
    public bool actionsInitialized = false;
    public Dictionary<ButtonDirectionsNames, T> Actions
    {
        get
        {
            if (!actionsInitialized)
            {
                initActions(Parent);
            }

            return actions;
        }
    }
    
    public Dictionary<ButtonDirectionsNames, T> actions =
        new Dictionary<ButtonDirectionsNames, T>();
    
    public T[] ActionsByIndex = new T[4];

    public ParentT Parent;
    
    public void initActions(ParentT tsc)
    {
        if (actionsInitialized == false) { 
            actions.Clear();
            actions.Add(ButtonDirectionsNames.North,ActionsByIndex[0]);
            actions.Add(ButtonDirectionsNames.East,ActionsByIndex[1]);
            actions.Add(ButtonDirectionsNames.South,ActionsByIndex[2]);
            actions.Add(ButtonDirectionsNames.West,ActionsByIndex[3]);
            Parent = tsc;
            if (Parent != null) {
                for (int i = 0; i < ActionsByIndex.Length; i++)
                {
                    ActionsByIndex[i].InitAction(tsc,i);
                }
                actionsInitialized = true;
            }
        }
    }
    
    private static IEnumerable<Type> GetTowerActions()
    {
        var q = typeof(T).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(T)));
        
        return q;
    }
}
