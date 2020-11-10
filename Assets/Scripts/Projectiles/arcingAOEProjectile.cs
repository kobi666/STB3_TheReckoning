using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcingAOEProjectile : OnTargetReachedProjectile
{
    RangeDetector RangeDetector;
    public RangeDetector RangeDetectorPrefab;
    private PoolObjectQueue<RangeDetector> rangeDetectorQueue = null;

    
    
    void OnDisable()
    {
        TargetBank?.Targets.Clear();
        base.OnDisable();
    }
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
            
            rdt.position = Vector2.MoveTowards(rdt.position,TargetPosition,Speed * 1.7f);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    protected void Awake()
    {
        base.Awake();
        
        rangeDetectorQueue = GameObjectPool.Instance.GetRangeDetectorQueue(RangeDetectorPrefab);
        if (RangeDetector == null) {
        RangeDetector = rangeDetectorQueue.Get();
        }
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
        onTargetPositionReached += delegate { PlayOnHitAnimation(null);};
        TargetBank = GetComponent<EffectableTargetBank>() ?? null;
        TargetBank.Detector = RangeDetector;
        TargetBank.InitRangeDetectorEvents();
        StartCoroutine(MoveRangeDetectorToTargetpositionAheadofself());
    }

    
    
    
    
}
