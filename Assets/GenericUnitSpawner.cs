using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBox;
using NUnit.Framework.Internal;
using Sirenix.OdinInspector;
using SpawningBehaviors;
using UnityEngine;

public class GenericUnitSpawner : TowerComponent
{
    [ShowInInspector]
    public Dictionary<string,SplinePathController> PathSplines = new Dictionary<string, SplinePathController>();
    
    
    [TypeFilter("getBehaviors")][SerializeReference]
    public SpawningBehavior SpawningBehavior;

    private IEnumerable<Type> getBehaviors()
    {
        return SpawningBehavior.GetBehaviors();
    }
    
    
    public PathPointFinder PathPointFinder;
    public Vector2 SpawningPoint;
    public Vector2? UnitSetPosition = null;
    public Vector2? ClosestPointToBase = null;
    public override void InitComponent()
    {
        
    }

    public event Action onPathUpdate;

    public override void PostAwake()
    {
        
    }

    protected void Awake()
    {
        base.Awake();
        PathPointFinder = GetComponent<PathPointFinder>();
        //  PathPointFinder.onPathFound +=
    }






    
    

    
    protected void Start()
    {
        base.Start();
        RangeDetector.UpdateSize(Data.componentRadius);
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

