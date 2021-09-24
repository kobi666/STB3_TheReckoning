using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System;


[System.Serializable]
public abstract class CursorActionsBase<TYPE,PARENT_TYPE> where TYPE : CursorActionBase<PARENT_TYPE>
{
    public Dictionary<ButtonDirectionsNames, TYPE> Actions
    {
        get
        {
            if (!actionsInitialized)
            {
                initActions(parentObject);
            }

            return actions;
        }
    }

    public abstract PARENT_TYPE parentObject { get; set; } 
    
    
    public Dictionary<ButtonDirectionsNames, TYPE> actions =
        new Dictionary<ButtonDirectionsNames, TYPE>();


    
    [TypeFilter("getActionsTypes")][SerializeReference]
    public TYPE[] ActionsByIndex = new TYPE[4];

   
    
    [Button]
    void north()
    {
        ActionsByIndex[0].ExecAction(ButtonDirectionsNames.North);
    }
    [Button]
    void east()
    {
        ActionsByIndex[1].ExecAction(ButtonDirectionsNames.East);
    }
    [Button]
    void south()
    {
        ActionsByIndex[2].ExecAction(ButtonDirectionsNames.South);
    }
    
    [Button]
    void west()
    {
        ActionsByIndex[3].ExecAction(ButtonDirectionsNames.West);
    }


    public bool actionsInitialized = false;
    public void initActions(PARENT_TYPE tsc)
    {
        if (actionsInitialized == false) { 
            actions.Clear();
            actions.Add(ButtonDirectionsNames.North,ActionsByIndex[0]);
            actions.Add(ButtonDirectionsNames.East,ActionsByIndex[1]);
            actions.Add(ButtonDirectionsNames.South,ActionsByIndex[2]);
            actions.Add(ButtonDirectionsNames.West,ActionsByIndex[3]);
            parentObject = tsc;
            if (parentObject != null) {
                for (int i = 0; i < ActionsByIndex.Length; i++)
                {
                    ActionsByIndex[i].InitAction(tsc,i);
                }
                actionsInitialized = true;
            }
            }
    }
    
    private static IEnumerable<Type> getActionsTypes()
    {
        var q = typeof(TYPE).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(TYPE)));
        return q;
    }
}