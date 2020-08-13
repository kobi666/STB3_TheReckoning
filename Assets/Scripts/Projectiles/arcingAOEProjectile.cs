using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcingAOEProjectile : OnTargetReachedProjectile
{
    public GameObject TestTarget;
    RangeDetector RangeDetector;
    public RangeDetector RangeDetectorPrefab;
    
    
    EffectableTargetBank TargetBank;
    
    public override void OnTargetReachedAction() {
        //SpriteRenderer.enabled = false;
        gameObject.SetActive(false);
        PlayOnHitAnimationAtPosition(null,TargetPosition);
        
    }

    public override void MovementFunction() {

    }
    public override void AdditionalOnDisableActions() {

    }

    public override void PostAwake() {

    }

    IEnumerator MoveRangeDetectorToTargetpositionAheadofself() {
        Transform rdt = RangeDetector.transform;
        rdt.position = (Vector2)transform.position - TargetPosition;
        while ((Vector2)rdt.position != TargetPosition) {
            
            rdt.position = Vector2.MoveTowards(rdt.position,TargetPosition,speed * 1.5f);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    protected void Awake()
    {
        PoolObjectQueue<RangeDetector> rdq = GameObjectPool.Instance.GetRangeDetectorQueue(RangeDetectorPrefab);
        RangeDetector = rdq.Get();
        base.Awake();
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
        TargetPosition = TestTarget.transform.position;
      MovementCoroutine = ProjectileUtils.MoveInArc(transform,TargetPosition, Data.ArcValue,Data.Speed,OnTargetReachedAction);
      base.Start();
      onTargetPositionReached += delegate { PlayOnHitAnimation(null);};
      TargetBank = GetComponent<EffectableTargetBank>() ?? null;
      TargetBank.rangeDetector = RangeDetector;
      TargetBank.InitRangeDetectorEvents();
      StartCoroutine(MoveRangeDetectorToTargetpositionAheadofself());
    }

    void OnDisable()
    {
        RangeDetector?.gameObject.SetActive(false);
        base.OnDisable();
    }
    
}
