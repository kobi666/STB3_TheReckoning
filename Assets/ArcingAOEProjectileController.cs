using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcingAOEProjectileController : AOEProjectile
{
    protected void OnEnable()
    {
        if (MovementCoroutine == null)
        {
            MovementCoroutine = ProjectileUtils.MoveInArcAndInvokeAction(transform, TargetPosition, Data.ArcValue,
                Data.Speed, OnTargetPositionReached);
        }
        StartCoroutine(MoveRangeDetectorToTargetpositionAheadofself());
        StartCoroutine(MovementCoroutine);
    }

    protected void OnDisable()
    {
        base.OnDisable();
    }
    
    IEnumerator MoveRangeDetectorToTargetpositionAheadofself()
    {
        if (RangeDetector != null) {
        Transform rdt = RangeDetector.transform;
        rdt.position = (Vector2)transform.position - TargetPosition;
        while ((Vector2)rdt.position != TargetPosition) {
            
            rdt.position = Vector2.MoveTowards(rdt.position,TargetPosition,Speed * 1.7f);
            yield return new WaitForFixedUpdate();
            }
        }
        yield break;
        }
    
    protected void Start()
    {
        base.Start();
        
    }

    

    // Update is called once per frame
    

        public override void MovementFunction()
        {
            
        }

        public override void AdditionalOnDisableActions()
        {
            
        }

        public override void PostAwake()
        {
            
        }
    }

