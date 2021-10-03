using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public abstract class CursorActionBaseBase
{
    public abstract void Action(ButtonDirectionsNames buttonDirectionsName);

    public abstract void ExecAction(ButtonDirectionsNames buttonDirectionsName);
    
    public abstract bool ExecutionConditions();

    public bool GeneralExecutionConditions()
    {
        if (ExecutionConditions())
        {
            return true;
        }
        return false;
    }
    
    public abstract int ActionCost { get; set; }
    
    public int ActionIndex;

    public ButtonDirectionsNames ButtonDirectionsName;


    public Color ActionColor;

    [PreviewField]
    public abstract Sprite ActionSprite(ButtonDirectionsNames buttonDirectionsName);
}


public abstract class CursorActionBase<PARENT_TYPE> : CursorActionBaseBase
{
    public abstract void InitAction(PARENT_TYPE p, int actionIndex);
    
    public event Action<CursorActionBase<PARENT_TYPE>> onActionExec;

    public void OnActionExec()
    {
        onActionExec?.Invoke(this);
    }

    

    
}
