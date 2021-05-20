using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TowerActionIndicator : MyGameObject
{
    public TowerAction TowerAction;
    [Required]
    public TowerActionSpriteProjector SpriteProjector;

    


    public void ProjectAction(TowerAction towerAction)
    {
        SpriteProjector.ActionSprite = towerAction.ActionSprite;
    }

    public bool EvaluateAction(TowerAction towerAction)
    {
        bool eval = false;
        if (towerAction.GeneralExecutionConditions())
        {
            eval = true;
        }

        return eval;
    }

    protected void Awake()
    {
        
    }
}
