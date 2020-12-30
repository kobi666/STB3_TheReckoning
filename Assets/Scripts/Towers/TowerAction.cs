using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Serialization;

[System.Serializable]
public class TowerAction
{
   [FormerlySerializedAs("ParentTowerController")] public TowerControllerLegacy parentTowerControllerLegacy;
   
   public virtual void InitAction(TowerControllerLegacy tc)
   {
      
   }

   public virtual bool ExecuteCondition()
   {
      return true;
   }

   public int Cost;
}


[System.Serializable][ShowOdinSerializedPropertiesInInspector]
public class UpgradeToNewTower : TowerAction
{
   [SerializeField][SerializeReference]
   public TowerController TowerObject;
   
   [SerializeField][ShowInInspector][SerializeReference]
   public int someInt;
   
   public override void InitAction(TowerControllerLegacy tc)
   {
      
   }

   public override bool ExecuteCondition()
   {
      return true;
   }
}

public class someTestAction : TowerAction
{
   public float testFloat;
}
