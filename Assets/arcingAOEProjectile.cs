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

    }

    public override void MovementFunction() {

    }
    public override void AdditionalOnDisableActions() {

    }

    public override void PostAwake() {

    }

    // IEnumerator MoveTargetDetectorToTargetPosition() {
    //     Transform rangeDetector = RangeDetector.transform;
    //     rangeDetector.position = (Vector2)transform.position - TargetPosition;
    //     while ((Vector2)rangeDetector.transform.position != TargetPosition) {
    //         rangeDetector.position = Vector2.MoveTowards(rangeDetector.position, TargetPosition, Data.Speed * 3);
    //         yield return new WaitForFixedUpdate();
    //     }
    //     yield break;
    // }

    IEnumerator MoveRangeDetectorToTargetpositionAheadofself() {
        Transform rdt = RangeDetector.transform;
        rdt.position = (Vector2)transform.position - TargetPosition;
        while ((Vector2)rdt.position != TargetPosition) {
            rdt.position = Vector2.MoveTowards(rdt.position,TargetPosition,speed * 10);
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
      
      TargetBank = GetComponent<EffectableTargetBank>() ?? null;
      TargetBank.rangeDetector = RangeDetector;
      TargetBank.InitRangeDetectorEvents();
      StartCoroutine(MoveRangeDetectorToTargetpositionAheadofself());
    }

    protected void Update() {
        
    }

    
    void OnDisable()
    {
        RangeDetector.gameObject.SetActive(false);
        base.OnDisable();
    }
    
}
