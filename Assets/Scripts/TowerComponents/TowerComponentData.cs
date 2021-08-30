using UnityEngine;

[System.Serializable]
public class TowerComponentData 
{
    public float componentRadius = 0;
    public Effectable effectableTarget = null;

    public GenericUnitController targetUnit = null;

    public TowerComponentOrbitalControllerData orbitalData;
    public TowerComponentUnitSpawnerData SpawnerData;
    public TowerComponentBeamData BeamData;
    [SerializeField]
    public DamageRange damageRange;
    public float fireRate;

}
