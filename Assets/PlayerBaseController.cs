using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerBaseController : MyGameObject
{
    
    
    [Required]
    public CollisionDetector CollisionDetector;

    public event Action<int> onEnemyEnter;
    public void OnEnemyEnter (int objectGID)
    {
        onEnemyEnter?.Invoke(objectGID);
    }

    protected void Awake()
    {
        CollisionDetector.onTargetEnter += OnEnemyEnter;
        onEnemyEnter += KillEnemyEntered;
            //        onEnemyEnter += delegate(int i) { Debug.LogWarning("Enemy Entered with collision ID" + i);   };
    }

    void KillEnemyEntered(int EnemyCollisionID)
    {
        int GID = GameObjectPool.CollisionIDToGameObjectID[EnemyCollisionID].Item1;
        if (GameObjectPool.Instance.ActiveUnits.ContainsKey(GID)) {
        var enemy = GameObjectPool.Instance.ActiveUnits[GID];
        int DamageToBase = enemy.DamageToBase;
        GameManager.Instance.UpdateLife(-DamageToBase);
        enemy.EffectableUnit.ApplyDamage(99999);
        }
        else
        {
            Debug.LogWarning("Got collision but unit not found");
        }
    }
    
    
}
