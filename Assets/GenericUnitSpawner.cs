using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using DegreeUtils;

using UnityEngine;

[RequireComponent(typeof(PathPointFinder))]
public class GenericUnitSpawner : TowerComponent
{
    [ShowInInspector]
    public Dictionary<string,SplinePathController> PathSplines = new Dictionary<string, SplinePathController>();

    private Dictionary<string,GenericUnitController> ManagedUnits = new Dictionary<string,GenericUnitController>();
    

    public bool MaxUnitsReached;
    private int numberOfManagedUnits;
    public int NumberOfManagedUnits
    {
        get => numberOfManagedUnits;
        set
        {
            numberOfManagedUnits = value;
            if (numberOfManagedUnits >= MaxUnits)
            {
                MaxUnitsReached = true;
            }
            else
            {
                MaxUnitsReached = false;
            }
        }
    }

    public event Action<int, string> onUnitAdd;

    public void OnUnitAdd(int NumOfManagedUnits, string newUnitName)
    {
        onUnitAdd?.Invoke(NumOfManagedUnits,newUnitName);
    }
    public void AddManagedUnit(GenericUnitController unit)
    {
        if (!ManagedUnits.ContainsKey(unit.name))
        {
            ManagedUnits.Add(unit.name,unit);
        }

        NumberOfManagedUnits = ManagedUnits.Count();

    }
    

    
    
    

    [TypeFilter("getBehaviors")][SerializeReference]
    public SpawnerBehavior SpawnerBehavior;

    private IEnumerable<Type> getBehaviors()
    {
        return SpawnerBehavior.GetBehaviors();
    }

    public GenericUnitController SpawnUnitInactive(PoolObjectQueue<GenericUnitController> _unitPool)
    {
        GenericUnitController guc = _unitPool.GetInactive();
        
        AddManagedUnit(guc);
        return guc;
    }
    
    
    public PathPointFinder PathPointFinder;
    public Vector2 SpawningPoint;
    public Vector2? UnitSetPosition = null;
    public Vector2? ClosestPointToBase = null;

    public event Action onMaxUnitsChanged;
    [OnValueChanged("OnMaxUnitsChanged")]
    public int maxUnits = 3;

    public int MaxUnits
    {
        get => maxUnits;
        set
        {
            onMaxUnitsChanged?.Invoke(); 
        }
    }

    private void OnMaxUnitsChanged()
    {
        onMaxUnitsChanged?.Invoke();
    }
    public override void InitComponent()
    {
        SpawnerBehavior.InitBehavior(this,PathPointFinder);
        string firstNameOrEmpty;

    }

    public event Action onPathUpdate;

    public override void PostAwake()
    {
        
    }

    protected void Awake()
    {
        base.Awake();
        PathPointFinder = GetComponent<PathPointFinder>();
    }

    void RemoveUnitFromManagedUnits(string unitName)
    {
        if (ManagedUnits.ContainsKey(unitName))
        {
            ManagedUnits.Remove(unitName);
        }

        NumberOfManagedUnits = ManagedUnits.Count();
    }



    protected void Start()
    {
        base.Start();
        RangeDetector.UpdateSize(Data.componentRadius);
        DeathManager.Instance.onUnitDeath += RemoveUnitFromManagedUnits;
        //GameObjectPool.Instance.ActiveUnits
        InitComponent();
        
    }

    
    
    

    

    public override List<Effect> GetEffectList()
    {
        return null;
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        
    }

    public override List<TagDetector> GetTagDetectors()
    {
        List<TagDetector> ltd = new List<TagDetector>();
        ltd.Add(RangeDetector);
        return ltd;
    }

    
    void Update()
    {
        if (UnitSetPosition != null)
        {
            Debug.DrawLine(transform.position, (Vector3)UnitSetPosition);
        }
    }
}



