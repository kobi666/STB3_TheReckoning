using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerComponentData 
{
    public float DistanceFromRotatorBase;
    public float OrbitingSpeed;
    public float RotationSpeed;
    public Transform OrbitBase;
    int numOfOrbitals;
    public int NumOfOrbitals {
        get => numOfOrbitals;
        set {
            numOfOrbitals = value;
        }
    }

    public OrbitalWeapon OrbitalGunPrefab;

    public int MaxNumberOfOrbitals;
    public Projectile ProjectilePrefab;
    
    [SerializeField]
    public DamageRange damageRange;
    public float FireRate;

    [SerializeField]
    EnemyUnitController enemyTarget;
    
    [SerializeField]
    public EnemyUnitController EnemyTarget {
        get { if (enemyTarget?.IsTargetable() ?? false) 
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
