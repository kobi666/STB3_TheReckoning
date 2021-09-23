using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[DefaultExecutionOrder(-10)]
public class CursorActionsController : MonoBehaviour
{
    
    [Required]
    public CursorActionIndicator[] TowerActionIndicators = new CursorActionIndicator[4];

    /*public TowerActions TowerActions;*/
    public float DistanceFromCursor = 1f;
    
    [ShowInInspector]
    public Dictionary<ButtonDirectionsNames, CursorActionHandler> CursorActionHandlers =
        new Dictionary<ButtonDirectionsNames, CursorActionHandler>();


    public void ExecuteAction(ButtonDirectionsNames buttonName)
    {
        if (GameManager.Instance.FreeActions) {
        CursorActionHandlers[buttonName].ExecuteAction();
        }
        else
        {
            var cursorActionHandler = CursorActionHandlers[buttonName];
            if (GameManager.Instance.Money >= cursorActionHandler.CurrentTowerAction.ActionCost)
            {
                if (cursorActionHandler.ActionAvailableCheck()) {
                GameManager.Instance.UpdateMoney(-cursorActionHandler.CurrentTowerAction.ActionCost);
                CursorActionHandlers[buttonName].ExecuteAction();
                }
            }
        }
    }

    public void UpdateTowerActions(TowerActions towerActions)
              
    {
        foreach (var towerAction in towerActions.Actions)
        {
            CursorActionHandlers[towerAction.Key].CurrentTowerAction = towerAction.Value;
            CursorActionHandlers[towerAction.Key].UpdateAction();
        }
    }

    protected void Awake()
    {
        for (int i = 0; i < CardinalSet.buttonDirectionNamesClockwiseArray.Length; i++)
        {
            var cah = new CursorActionHandler(this, TowerActionIndicators[i]);
            CursorActionHandlers.Add(CardinalSet.buttonDirectionNamesClockwiseArray[i], cah);
            var v2 = TowerUtils.Cardinal4.directionsClockwise[i] * DistanceFromCursor;
            cah.cursorActionIndicator.SpriteProjector.transform.localPosition =v2;
        }

        InputManager.Instance.onActionButtonPress += ExecuteAction;
    }
}


[System.Serializable]
public class CursorActionHandler
{
    private CursorActionsController ParentCursorActionsController;
    [FormerlySerializedAs("TowerActionIndicator")] public CursorActionIndicator cursorActionIndicator;
    public CursorActionBaseBase CurrentTowerAction;

    private bool actionState = false;
    private bool previousActionState = false;

    
    

    public bool ActionAvailableCheck()
    {
        if (CurrentTowerAction != null)
            {
                if (CurrentTowerAction.GeneralExecutionConditions())
                {
                    previousActionState = actionState;
                    actionState = true;
                    return true;
                }
            }

        previousActionState = actionState;
        actionState = false;
        return false;
    }

    public void ExecuteAction()
    {
        if (ActionAvailableCheck())
        {
            CurrentTowerAction.ExecAction();
            UpdateAction();
        }
    }

    public async void UpdateAction()
    {
        await Task.Yield();
        if (CurrentTowerAction != null)
        {
            if (ActionAvailableCheck())
            {
                cursorActionIndicator.SpriteProjector.SetSprite(CurrentTowerAction, true);
            }
            else
            {
                cursorActionIndicator.SpriteProjector.DisableProjector();
            }
        }
        else
        {
            cursorActionIndicator.SpriteProjector.DisableProjector();
        }
    }

    public CursorActionHandler(CursorActionsController parentCursorActionsController, CursorActionIndicator cursorActionIndicator)
    {
        ParentCursorActionsController = parentCursorActionsController;
        this.cursorActionIndicator = cursorActionIndicator;
        UpdateAction();
    }
    
    
}


public enum ButtonDirectionsNames
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}
