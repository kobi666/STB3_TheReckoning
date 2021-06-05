using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine.Serialization;





[System.Serializable]
public abstract class TowerAction
{
   public static IEnumerable<Type> GetTowerActions()
   {
      var q = typeof(TowerAction).Assembly.GetTypes()
         .Where(x => !x.IsAbstract) // Excludes BaseClass
         .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
         .Where(x => typeof(TowerAction).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
      return q;
   }


   public Color ActionColor;
   public Sprite actionSprite;
   public virtual Sprite ActionSprite { get => actionSprite; set => actionSprite = value; }
   
   public TowerSlotController ParentSlotController;

   public void InitAction(TowerSlotController tsc)
   {
      /*if (tsc == null)
      {
         Debug.LogError("TSC IS NULL");
      }*/

      ParentSlotController = ParentSlotController != null ? ParentSlotController : tsc;
   }

   public abstract void InitActionSpecific();

   public int ActionCost;
   
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



   public event Action<TowerAction> onActionExec;

   public void OnActionExec()
   {
      onActionExec?.Invoke(this);
   }
   public abstract void Action();

   public void ExecAction()
   {
      if (GeneralExecutionConditions())
      {
         Action();
         OnActionExec();
         ActionCounter++;
      }
   }

   public abstract bool ExecutionConditions();

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
   public override void InitActionSpecific()
   {
      
   }

   public override void Action()
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
   public override void InitActionSpecific()
   {
      
   }

   public override void Action()
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

public class NullAction : TowerAction
{
   public override void InitActionSpecific()
   {
      
   }

   public override void Action()
   {
      
   }

   public override bool ExecutionConditions()
   {
      return false;
   }
}

