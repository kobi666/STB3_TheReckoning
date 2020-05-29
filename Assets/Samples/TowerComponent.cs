using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public Tower ParentTower;
    public EnemyTargetBank TargetBank {get ; private set;}

    private void Start() {
        TargetBank = GetComponentInChildren<EnemyTargetBank>() ?? ParentTower?.TargetBank ?? null;
        PostStart();
    }
    public abstract void PostStart();
}
