using UnityEngine;

[System.Serializable]
public class TowerComponentData 
{
    public float componentRadius = 0;
    public Effectable effectableTarget = null;

    public TargetUnit targetUnit = null;

    public TowerComponentOrbitalControllerData orbitalData;
    public TowerComponentUnitSpawnerData SpawnerData;
    public TowerComponentBeamData BeamData;
    [SerializeField]
    public DamageRange damageRange;
    public float fireRate;

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
