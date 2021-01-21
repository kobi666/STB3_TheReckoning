using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Serialization;

[System.Serializable]
public class TowerAction
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
   public TowerComponent TowerComponent;

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
      if (TowerComponent == null)
      {
         InitActionSpecific(tsc);
      }
      else
      {
         InitActionSpecific(tsc);
         onInitAction += InitActionSpecific;
         TowerComponent.onInitComponent += OnInitAction;
      }
   }

   public virtual void InitActionSpecific(TowerSlotController tsc)
   {
      
   }
   
   

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

   public virtual void Action(TowerSlotController tsc)
   {
      
   }

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
public class UpgradeToNewTower : TowerAction
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
   
   [ShowInInspector] private List<Effect> DamageEffects = new List<Effect>();
   public Damage DamageUpdate = new Damage();
   public override void InitActionSpecific(TowerSlotController tsc)
   {
      var q = TowerComponent.GetEffectList().Where(x => x.EffectName() == DamageUpdate.EffectName()).ToList();
      DamageEffects = q;
   }

   public override void Action(TowerSlotController tsc)
   {
      TowerComponent.UpdateEffect(DamageUpdate, DamageEffects);
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
         towerAction.TowerComponent = TowerComponent;
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
      DetectorsToApplyOn = TowerComponent.GetRangeDetectors();
   }

   public override void Action(TowerSlotController tsc)
   {
      TowerComponent.UpdateRange(RangeUpdate,DetectorsToApplyOn);
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return base.SpecificExecuteCondition(tsc);
   }
}






