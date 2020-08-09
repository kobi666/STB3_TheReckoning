using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcingAOEProjectile : OnTargetReachedProjectile
{

    RangeDetector rangeDetector;
    EffectableTargetBank TargetBank;
    public override IEnumerator MovementCoroutine { get => movementCoroutine ; set { movementCoroutine = value;}}
    public override void OnTargetReachedAction() {

    }

    public override void MovementFunction() {

    }
    public override void AdditionalOnDisableActions() {

    }

    public override void PostAwake() {

    }

    
    protected void Awake()
    {
        base.Awake();
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
      rangeDetector = GetComponentInChildren<RangeDetector>() ?? null;
      TargetBank = GetComponent<EffectableTargetBank>() ?? null;
      rangeDetector.transform.position = new Vector2(TargetPosition.x)  
      base.Start();
    }

    // Update is called once per frame
    
}
