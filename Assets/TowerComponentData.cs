﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerComponentData 
{
    public float distanceFromRotatorBase;
    public float RoatationSpeed;
    int numOfOrbitals;
    public int NumOfOrbitals {
        get => numOfOrbitals;
        set {
            numOfOrbitals = value;
        }
    }

    public OrbitalWeapon OrbitalGunPrefab;

    public int MaxNumberOfOrbitals;
    public Projectile projectilePrefab;
    
    [SerializeField]
    public DamageRange damageRange;
    public float FireRate;

    EnemyUnitController enemyTarget;
    
    [SerializeField]
    public EnemyUnitController EnemyTarget {
        get { if (enemyTarget.IsTargetable()) 
            {
                return enemyTarget;
            }
            else
            {
                return null;
            }
}
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
