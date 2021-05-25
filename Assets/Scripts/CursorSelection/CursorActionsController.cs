using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
[DefaultExecutionOrder(-10)]
public class CursorActionsController : MonoBehaviour
{
    
    [Required]
    public TowerActionIndicator[] TowerActionIndicators = new TowerActionIndicator[4];

    /*public TowerActions TowerActions;*/
    public float DistanceFromCursor = 1f;
    
    [ShowInInspector]
    public Dictionary<ButtonDirectionsNames, CursorActionHandler> CursorActionHandlers =
        new Dictionary<ButtonDirectionsNames, CursorActionHandler>();


    public void ExecuteAction(ButtonDirectionsNames buttonName)
    {
        CursorActionHandlers[buttonName].ExecuteAction();
    }

    public void UpdateTowerActions(TowerActions towerActions)
              
    {
        foreach (var towerAction in towerActions.Actions)
        {
            CursorActionHandlers[towerAction.Key].CurrentTowerAction = towerAction.Value;
            CursorActionHandlers[towerAction.Key].UpdateAction();
        }
    }

    protected void Start()
    {
        for (int i = 0; i < CardinalSet.buttonDirectionNamesClockwiseArray.Length; i++)
        {
            var cah = new CursorActionHandler(this, TowerActionIndicators[i]);
            CursorActionHandlers.Add(CardinalSet.buttonDirectionNamesClockwiseArray[i], cah);
            var v2 = TowerUtils.Cardinal4.directionsClockwise[i] * DistanceFromCursor;
            cah.TowerActionIndicator.SpriteProjector.transform.localPosition =v2;
        }

        InputManager.Instance.onActionButtonPress += ExecuteAction;
    }
}


[System.Serializable]
public class CursorActionHandler
{
    private CursorActionsController ParentCursorActionsController;
    public TowerActionIndicator TowerActionIndicator;
    public TowerAction CurrentTowerAction;

    private bool actionState = false;
    private bool previousActionState = false;

    
    

    private bool ActionAvailableCheck()
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
                TowerActionIndicator.SpriteProjector.SetSprite(CurrentTowerAction, true);
            }
            else
            {
                TowerActionIndicator.SpriteProjector.DisableProjector();
            }
        }
        else
        {
            TowerActionIndicator.SpriteProjector.DisableProjector();
        }
    }

    public CursorActionHandler(CursorActionsController parentCursorActionsController, TowerActionIndicator towerActionIndicator)
    {
        ParentCursorActionsController = parentCursorActionsController;
        TowerActionIndicator = towerActionIndicator;
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
