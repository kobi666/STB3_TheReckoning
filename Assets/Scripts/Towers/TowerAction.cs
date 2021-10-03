using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine.Serialization;





[System.Serializable]
public abstract class TowerAction : CursorActionBase<TowerSlotController>
{
   public static IEnumerable<Type> GetTowerActions()
   {
      var q = typeof(TowerAction).Assembly.GetTypes()
         .Where(x => !x.IsAbstract) // Excludes BaseClass
         .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
         .Where(x => typeof(TowerAction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
      return q;
   }
   
   [SerializeField]
   public TowerSlotController ParentSlotController;

   public override void InitAction(TowerSlotController tsc, int actionIndex)
   {
      ActionIndex = actionIndex;
      ParentSlotController = ParentSlotController != null ? ParentSlotController : tsc;
   }

   public abstract void InitActionSpecific();
   
   
   
   [ShowInInspector]
   public int ActualActionCost
   {
      get
      {
         if (GameManager.Instance?.FreeActions ?? false)
         {
            return 0;
         }
         else
         {
            return ActionCost + (actionConter * ActionCostIncrement);
         }
      }
   }


   [SerializeField] private int actionConter;

   public event Action maxActionsReached;
   public int ActionCounter
   {
      get => actionConter;
      set
      {
         if (value <= MaxActions)
         {
            actionConter = value;
            if (actionConter >= MaxActions)
            {
               maxActionsReached?.Invoke();
            }
         }
         
      }
   }

   public int MaxActions = 1;

   public int ActionCostIncrement = 0;
   
   //it is possible to chain actions one after the other by adding another TowerAction with serializeReference as another object here,
   //and then initializing it and switching it on maxActionsReached.



   

   
   

   public override void ExecAction(ButtonDirectionsNames buttonDirectionsNames)
   {
      if (GeneralExecutionConditions())
      {
         Action(buttonDirectionsNames);
         OnActionExec();
         ActionCounter++;
      }
   }

   

   public bool GeneralExecutionConditions()
   {
      if (ActionCounter < MaxActions) {
         if (ExecutionConditions())
         {
            return true;
         }
      }

      return false;
   }
}

public class TestTowerAction : TowerAction
{
   public override Sprite ActionSprite(ButtonDirectionsNames buttonDirectionsName)
   {
      return null;
   }

   public override void InitActionSpecific()
   {
      
   }

   public int actionCost;
   public override int ActionCost { get => actionCost; set => actionCost = value; }

   public override void Action(ButtonDirectionsNames buttonDirectionsName)
   {
      
   }

   public override bool ExecutionConditions()
   {
      return true;
   }
}


public class PlaceNewTowerFromPrefab : TowerAction
{
   [Required] public TowerController TowerPrefab;

   [PreviewField, ShowInInspector]
   public override Sprite ActionSprite(ButtonDirectionsNames buttonDirectionsName)
   {
      return TowerPrefab.TowerSprite;
   }

   public override void InitActionSpecific()
   {
      
   }

   public int actionCost = 50;
   
   public override int ActionCost { get => actionCost; set => actionCost = value; }

   public override void Action(ButtonDirectionsNames buttonDirectionsName)
   {
      try
      {
         ParentSlotController.PlaceNewTower(TowerPrefab);
      }
      catch (Exception e)
      {
         Debug.LogWarning(e);
         throw;
      }
      
   }

   public override bool ExecutionConditions()
   {
      return true;
   }
}

public class PlaceNewTowerFromGameManager : TowerAction
{
   public override Sprite ActionSprite(ButtonDirectionsNames buttonDirectionsName)
   {
      return TowerController.TowerSprite;
   }

   public TowerController TowerController
   {
      get => GameManager.Instance?.PlayerTowersManager.GetTowerByButtonName(ButtonDirectionsName) ?? null;
   }

   public override void InitActionSpecific()
   {
      
   }

   public override int ActionCost { get => TowerController?.DefaultTowerCost ?? 10; set => ActionCost = ActionCost; }
   public override void Action(ButtonDirectionsNames buttonDirectionsName)
   {
      try
      {
         ParentSlotController.PlaceNewTower(TowerController);
      }
      catch (Exception e)
      {
         Debug.LogWarning(e);
         throw;
      }
   }

   public override bool ExecutionConditions()
   {
      return (TowerController != null);
   }
}

public class NullAction : TowerAction
{
   [PreviewField]
   public Sprite actionSprite;

   public override Sprite ActionSprite(ButtonDirectionsNames buttonDirectionsName)
   {
      return null;
   }

   public override void InitActionSpecific()
   {
      
   }
   
   

   private int actionCost = 0;
   public override int ActionCost { get => actionCost; set => actionCost = value; }

   public override void Action(ButtonDirectionsNames buttonDirectionsNames)
   {
      
   }

   public override bool ExecutionConditions()
   {
      return false;
   }
}

