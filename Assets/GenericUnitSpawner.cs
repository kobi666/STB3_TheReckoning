using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBox;
using NUnit.Framework.Internal;
using Sirenix.OdinInspector;
using UnityEngine;

public class GenericUnitSpawner : TowerComponent
{
    [ShowInInspector]
    public Dictionary<string,SplinePathController> PathSplines = new Dictionary<string, SplinePathController>();
    
    
    public UnitPoolCreationData UnitPoolCreationData = new UnitPoolCreationData();

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






    public IEnumerator WaitAlittleAndFindMiddlePoint()
    {
        yield return new WaitForEndOfFrame();
        UnitSetPosition = UnitSetPosition ?? PathPointFinder.GetPathPointByPriority();
        yield break;
    }
    

    
    protected void Start()
    {
        base.Start();
        RangeDetector.UpdateSize(Data.componentRadius);
        StartCoroutine(WaitAlittleAndFindMiddlePoint());
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

