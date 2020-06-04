using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

public abstract class TowerComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer SR;
    public AnimancerComponent Animancer;
    public TowerComponentData Data;
    public Tower ParentTower;
    public EnemyTargetBank TargetBank {get ; private set;}

    public abstract event Action<EnemyUnitController> onEnemyEnteredRange;
    public abstract void OnEnemyEnteredRange(EnemyUnitController ec);

        

    private void Awake() {
        TargetBank = GetComponentInChildren<EnemyTargetBank>() ?? ParentTower?.TargetBank ?? null;
        SR = GetComponent<SpriteRenderer>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
        PostAwake();
    }

    public abstract void PostAwake();
    
}
