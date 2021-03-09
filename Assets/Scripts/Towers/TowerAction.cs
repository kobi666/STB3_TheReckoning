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
   
   
   
   
   [FormerlySerializedAs("ParentTowerController")] public TowerControllerLegacy parentTowerControllerLegacy;
   public TowerSlotController ParentSlotController;
   public TowerComponent towerComponent;

   public event Action<TowerSlotController> onInitAction;

   public void OnInitAction()
   {
      onInitAction?.Invoke(ParentSlotController);
   }
   public void InitAction(TowerSlotController tsc)
   {
      ParentSlotController = tsc;
      PlayerResources.Instance.onMoneyzUpdate += EnoughMoneyzCheck;
      //PlayerResources.Instance.UpdateMoneyz(0);
      if (towerComponent == null)
      {
         InitActionSpecific(tsc);
      }
      else
      {
         InitActionSpecific(tsc);
         onInitAction += InitActionSpecific;
         towerComponent.onInitComponent += OnInitAction;
      }
   }

   public abstract void InitActionSpecific(TowerSlotController tsc);



   private void EnoughMoneyzCheck(int moneyz)
   {
      if (moneyz >= Cost)
      {
         PlayerHasEnoughMoney = true;
      }
      else
      {
         PlayerHasEnoughMoney = false;
      }
   }

   public void onActionTerminate(TowerSlotController tsc)
   {
      PlayerResources.instance.onMoneyzUpdate -= EnoughMoneyzCheck;
   }
   
   

   private bool PlayerHasEnoughMoney = true;

   private bool ActionCounterIsZero
   {
      get
      {
         if (actionCounter < MaxActions)
         {
            return true;
         }

         return false;
      }
   }

   public bool CanActionExecute()
   {
      if (PlayerHasEnoughMoney)
      {
         if (ActionCounterIsZero)
         {
            if (SpecificExecuteCondition(ParentSlotController) == true)
            {
               return true;
            }
         }
      }

      return false;
   }

   public abstract void Action(TowerSlotController tsc);

   public void ExecuteAction(TowerSlotController tsc)
   {
      if (CanActionExecute())
      {
         Action(tsc);
      }
      actionCounter += 1;
   }

   public virtual bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return true;
   }

   public int Cost = 0;
   public float ActionTimer = 0;
   [ShowInInspector]
   private float actionCounter = 0;
   public float MaxActions = 5;

}


[System.Serializable]
public class UpgradeToNewTowerLegacy : TowerAction
{
   [SerializeField]
   public TowerController UpgradeTowerObject;

   public override void InitActionSpecific(TowerSlotController tsc)
   {
      
   }

   public override void Action(TowerSlotController tsc)
   {
      tsc.PlaceNewTowerLegacy(UpgradeTowerObject.gameObject);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return true;
   }
}

public class UpdateDamage : TowerAction
{
   
   private List<Effect> DamageEffects = new List<Effect>();
   public Damage DamageUpdate = new Damage();
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      var q = towerComponent.GetEffectList().Where(x => x.EffectName() == DamageUpdate.EffectName()).ToList();
      DamageEffects = q;
   }

   public override void Action(TowerSlotController tsc)
   {
      towerComponent.UpdateEffect(DamageUpdate, DamageEffects);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return true;
   }
}


[Serializable]
public class CompundAction : TowerAction
{
   [SerializeReference][TypeFilter("getTowerActions")]
   public List<TowerAction> Actions = new List<TowerAction>();
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      foreach (var towerAction in Actions)
      {
         towerAction.towerComponent = towerComponent;
         towerAction.InitAction(tsc);
      }
   }

   private static IEnumerable<Type> getTowerActions()
   {
      return TowerAction.GetTowerActions();
   }
   
   

   public override void Action(TowerSlotController tsc)
   {
      foreach (var ta in Actions)
      {
         ta.ExecuteAction(tsc);
      }
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return true;
   }
}


[Serializable]
public class UpdateRange : TowerAction
{
   private List<TagDetector> DetectorsToApplyOn;
   public float RangeUpdate = 0.5f;
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      DetectorsToApplyOn = towerComponent.GetTagDetectors();
   }

   public override void Action(TowerSlotController tsc)
   {
      towerComponent.UpdateRange(RangeUpdate,DetectorsToApplyOn);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return base.SpecificExecuteCondition(tsc);
   }
}


[Serializable]
public class PlaceNewTower : TowerAction
{
   public TowerController TowerPrefab;
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      
   }

   public override void Action(TowerSlotController tsc)
   {
      ParentSlotController.PlaceNewTower(TowerPrefab);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return base.SpecificExecuteCondition(tsc);
   }
}

public class PlaceNorthTower : TowerAction
{
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      Cost = TowerArsenal.arsenal.BaseTowerNorth.Cost;
   }

   public override void Action(TowerSlotController tsc)
   {
      ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.BaseTowerNorth.TowerPrefab);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return base.SpecificExecuteCondition(tsc);
   }
}

public class PlaceEastTower : TowerAction
{
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      Cost = TowerArsenal.arsenal.BaseTowerEast.Cost;
   }

   public override void Action(TowerSlotController tsc)
   {
      ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.BaseTowerEast.TowerPrefab);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return base.SpecificExecuteCondition(tsc);
   }
}

public class PlaceSouthTower : TowerAction
{
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      Cost = TowerArsenal.arsenal.BaseTowerSouth.Cost;
   }

   public override void Action(TowerSlotController tsc)
   {
      ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.BaseTowerSouth.TowerPrefab);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return base.SpecificExecuteCondition(tsc);
   }
}

public class PlaceWestTower : TowerAction
{
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      Cost = TowerArsenal.arsenal.BaseTowerWest.Cost;
   }

   public override void Action(TowerSlotController tsc)
   {
      ParentSlotController.PlaceNewTower(TowerArsenal.arsenal.BaseTowerWest.TowerPrefab);
   }
}






