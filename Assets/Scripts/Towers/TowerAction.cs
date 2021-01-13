using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Serialization;

[System.Serializable]
public class TowerAction
{
   [FormerlySerializedAs("ParentTowerController")] public TowerControllerLegacy parentTowerControllerLegacy;
   public TowerSlotController ParentSlotController;
   
   public virtual void InitAction(TowerSlotController tsc)
   {
      ParentSlotController = tsc;
      PlayerResources.instance.onMoneyzUpdate += EnoughMoneyzCheck;
      PlayerResources.instance.UpdateMoneyz(0);
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
   
   

   private bool PlayerHasEnoughMoney = false;
   private bool ActionCounterIsZero = true;

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
   }

   public virtual bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      return true;
   }

   public int Cost = 0;
   public float ActionTimer = 0;
   private float ActionCounter = 0;
   
}


[System.Serializable]
public class UpgradeToNewTower : TowerAction
{
   [SerializeField]
   public TowerController UpgradeTowerObject;

   public override void InitAction(TowerSlotController tsc)
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

[System.Serializable]
public class BuffDamage : TowerAction
{
   public DamageRange BuffValues = new DamageRange();
   private int actionCounter = 0;
   public int maxActions = 1;
   public List<Damage> damageEffects = new List<Damage>();
   public override void InitAction(TowerSlotController tsc)
   {
      Damage damage = new Damage();
      List<Effect> efs = tsc.ChildTower.WeaponController.WeaponAttack.GetEffects();
      foreach (var ef in efs)
      {
         if (ef.Effectname() == damage.Effectname())
         {
            damageEffects.Add(ef as Damage);
         }
      }
   }

   public override void Action(TowerSlotController tsc)
   {
      foreach (var de in damageEffects)
      {
         de.DamageRange.min += BuffValues.min;
         de.DamageRange.max += BuffValues.max;
         actionCounter += 1;
      }
   }

   public override bool SpecificExecuteCondition(TowerSlotController tsc)
   {
      if (actionCounter < maxActions)
      {
         return true;
      }

      return false;
   }
}


