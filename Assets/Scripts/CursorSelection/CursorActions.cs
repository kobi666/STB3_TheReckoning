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
    
    [SerializeReference]
    public Dictionary<ButtonDirectionsNames, T> actions =
        new Dictionary<ButtonDirectionsNames, T>();
    
    [SerializeReference]
    public T[] ActionsByIndex = new T[4];
    
    public abstract T Action_North { get; set; }
    public abstract T Action_East { get; set; }
    public abstract T Action_South { get; set; }
    public abstract T Action_West { get; set; }
    
    [SerializeReference]
    public ParentT Parent;
    
    public void initActions(ParentT parentObject)
    {
        if (actionsInitialized == false) { 
            actions.Clear();
            actions.Add(ButtonDirectionsNames.North,Action_North);
            actions.Add(ButtonDirectionsNames.East,Action_East);
            actions.Add(ButtonDirectionsNames.South,Action_South);
            actions.Add(ButtonDirectionsNames.West,Action_West);
            Parent = parentObject;
            if (Parent != null) {
                for (int i = 0; i < ActionsByIndex.Length; i++)
                {
                    ActionsByIndex[i]?.InitAction(parentObject,i);
                }
                actionsInitialized = true;
            }
        }
    }

    public abstract IEnumerable<Type> GetActions();
}
