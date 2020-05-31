using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerComponentData 
{
    int numOfOrbitals;
    public int NumOfOrbitals {
        get => numOfOrbitals;
        set {
            numOfOrbitals = value;
        }
    }
    public Projectile projectilePrefab;
    
    [SerializeField]
    public DamageRange damageRange;
    public float FireRate;

    EnemyUnitController enemyTarget;
    
    [SerializeField]
    public EnemyUnitController EnemyTarget {
        get => enemyTarget;
        set {
            enemyTarget = value;
        }
    }

    [SerializeField]
    PlayerUnitController playerTarget;

    public PlayerUnitController PlayerTarget {
        get => playerTarget;
        set {
            playerTarget = value;
        }
    }
    
}
