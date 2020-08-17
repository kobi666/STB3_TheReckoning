using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ArcingAOEProjectileController : AOEProjectile
{
    public Vector2 initToMiddle;
    public Vector2 MiddleToTarget;
    public Vector2 InitPos;
    public Vector2 MiddlePos;
    [SerializeField] private float moveCounter;
    
    protected void OnEnable()
    {
        TargetBank.RangeDetector.gameObject.SetActive(true);
        
        InitPos = transform.position;
        MiddlePos = ProjectileUtils.GetArcingMiddlePosition(InitPos, TargetPosition, Data.ArcValue);
        TargetBank.RangeDetector.transform.parent =
            GameObjectPool.Instance.PlaceHoldersDict[QueuePool.PlaceholderName].transform;
        TargetBank.RangeDetector.transform.localScale = new Vector3(Data.EffectRadius,Data.EffectRadius,Data.EffectRadius);
    }

    protected void OnDisable()
    {
        TargetBank.RangeDetector.gameObject.SetActive(false);
        TargetBank.RangeDetector.transform.localScale = new Vector3(1,1,1);
        moveCounter = 0;
        base.OnDisable();
    }
    
    IEnumerator MoveRangeDetectorToTargetpositionAheadofself()
    {
        if (RangeDetector != null) {
        Transform rdt = TargetBank.RangeDetector.transform;
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
        //onTargetPositionReached += delegate { Debug.LogWarning("Reached Position");};
        //onTargetPositionReached += delegate { Debug.LogWarning("Targets count: " + TargetBank.Targets.Count); };
        

    }

    

    // Update is called once per frame
    

        public override void MovementFunction()
        {
            ProjectileUtils.MoveInArcToPosition(transform,InitPos,MiddlePos,TargetPosition,ref initToMiddle,ref MiddleToTarget,Data.Speed,ref moveCounter);
            TargetBank.RangeDetector.transform.position =
                Vector2.MoveTowards((Vector2) transform.position, TargetPosition, Speed * 3);
            transform.Rotate(Vector3.forward * 380 * Time.deltaTime , Space.Self);
        }

        public override void AdditionalOnDisableActions()
        {
            
        }

        public override void PostAwake()
        {
            
        }
    }

