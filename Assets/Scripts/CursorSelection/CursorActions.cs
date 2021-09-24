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
    
    [SerializeReference]
    public ParentT Parent;
    
    public void initActions(ParentT parentObject)
    {
        if (actionsInitialized == false) { 
            actions.Clear();
            actions.Add(ButtonDirectionsNames.North,ActionsByIndex[0]);
            actions.Add(ButtonDirectionsNames.East,ActionsByIndex[1]);
            actions.Add(ButtonDirectionsNames.South,ActionsByIndex[2]);
            actions.Add(ButtonDirectionsNames.West,ActionsByIndex[3]);
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
