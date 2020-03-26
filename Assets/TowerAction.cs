using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerAction 
{
    public string ActionDescription = "Empty Action";
    public Sprite ActionSprite = null;
    public Action ActionFunction = null;

    public event Action AdditionalOptionalFunctions = null;

    public void Exec() {
        if (ActionFunction != null) {
            ActionFunction.Invoke();
            AdditionalOptionalFunctions?.Invoke();
        }
    }

    public TowerAction(string desc, Action action) {
        ActionDescription = desc;
        ActionFunction = action;
    }

    public TowerAction() {
        ActionDescription = "Empty Action";
        ActionFunction = null;
    }
   
}
