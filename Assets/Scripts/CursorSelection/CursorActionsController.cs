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
        CursorActionHandlers[buttonName].ExecuteAction(buttonName);
        }
        else
        {
            var cursorActionHandler = CursorActionHandlers[buttonName];
            if (GameManager.Instance.Money >= cursorActionHandler.CurrentTowerAction.ActionCost)
            {
                if (cursorActionHandler.ActionAvailableCheck()) {
                GameManager.Instance.UpdateMoney(-cursorActionHandler.CurrentTowerAction.ActionCost);
                CursorActionHandlers[buttonName].ExecuteAction(buttonName);
                }
            }
        }
    }

    public void UpdateActions(TowerActions towerActions)
              
    {
        foreach (var towerAction in towerActions.Actions)
        {
            CursorActionHandlers[towerAction.Key].CurrentTowerAction = towerAction.Value;
            CursorActionHandlers[towerAction.Key].UpdateAction(towerAction.Key);
        }
    }
    
    public void UpdateActions(LootActions lootActions) 
    {
        foreach (var towerAction in lootActions.Actions)
        {
            CursorActionHandlers[towerAction.Key].CurrentTowerAction = towerAction.Value;
            CursorActionHandlers[towerAction.Key].UpdateAction(towerAction.Key);
        }
    }

    protected void Awake()
    {
        for (int i = 0; i < CardinalSet.buttonDirectionNamesClockwiseArray.Length; i++)
        {
            var cah = new CursorActionHandler(this, TowerActionIndicators[i],CardinalSet.buttonDirectionNamesClockwiseArray[i]);
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

    public void ExecuteAction(ButtonDirectionsNames buttonDirectionsName)
    {
        if (ActionAvailableCheck())
        {
            CurrentTowerAction.ExecAction(buttonDirectionsName);
            UpdateAction(buttonDirectionsName);
        }
    }

    public async void UpdateAction(ButtonDirectionsNames buttonName)
    {
        await Task.Yield();
        if (CurrentTowerAction != null)
        {
            if (ActionAvailableCheck())
            {
                cursorActionIndicator.SpriteProjector.SetSprite(CurrentTowerAction,buttonName, true);
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

    public CursorActionHandler(CursorActionsController parentCursorActionsController, CursorActionIndicator cursorActionIndicator,ButtonDirectionsNames buttonName)
    {
        ParentCursorActionsController = parentCursorActionsController;
        this.cursorActionIndicator = cursorActionIndicator;
        UpdateAction(buttonName);
    }
    
    
}


public enum ButtonDirectionsNames
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
    None = 99
}
