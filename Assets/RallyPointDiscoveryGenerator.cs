using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BezierSolution;
using UnityEngine;


[RequireComponent(typeof(BezierSpline))]
public class RallyPointDiscoveryGenerator : MonoBehaviour
{
    public PathDiscoveryPoint PathDiscoveryPointPrefab;
    private BezierSpline spline;
    public PoolObjectQueue<PathDiscoveryPoint> PathDiscoveryPointPool;
    private float SplineLength;
    private float SplineNormalizedTRatioToDistance;
    public float DistanceBetweenPoints;
    private float NormalizedTLeaps;

    public event Action onPathCalculation;

    public void OnPathCalculation()
    {
        onPathCalculation?.Invoke();
    }

    public void  GetApproximateSplineLength()
    {
         float d =Vector2.Distance(spline.GetPoint(0),spline.GetPoint(0.001f));
         float ratio = 1 / d;
         SplineNormalizedTRatioToDistance = 1 / ratio;
         
        SplineLength = SplineNormalizedTRatioToDistance * 1000;
        NormalizedTLeaps = ( DistanceBetweenPoints / d ) / d;
    }
    
    protected void Awake()
    {
        onPathCalculation += GetApproximateSplineLength;
        spline = GetComponent<BezierSpline>();
    }


    public GameObject pathphobject;

    public IEnumerator movePathObjectsAndTriggerDetection(PathDiscoveryPoint[] pdps)
    {
        float time = 2f;
        Transform t = pathphobject.transform;
        Vector2 targetPos = t.position;
        /*t.position = (Vector2)t.position + Vector2.left * 5f;
        while (time > 0)
        {
            t.position = Vector2.MoveTowards(t.position, targetPos, 1);
            time -= StaticObjects.instance.DeltaGameTime;
            yield return null;
        }*/
        Dictionary<string, (Vector2,TowerSlotController)> towerSlotControllers = TowerSlotParentManager.instance.TowerslotControllers;
        
        foreach (var pd in pdps)
        {
            if (pd == null)
            {
                break;
            }
            pd.gameObject.SetActive(false);
            pd.gameObject.SetActive(true);
            yield return null;
        }
        foreach ((Vector2,TowerSlotController) slot in towerSlotControllers.Values)
        {
            slot.Item2.OnPathDiscoveryEvent();
        }
        
        yield break;

    }
    protected void Start()
    {
        if (DistanceBetweenPoints <= 0)
        {
            DistanceBetweenPoints = 0.3f;
        }
        
        
        //OnPathCalculation();
        NormalizedTLeaps = (1f / 250f);
        PathDiscoveryPointPool = GameObjectPool.Instance.GetPathDiscoveryPool(PathDiscoveryPointPrefab);
        PathDiscoveryPoint[] pds = new PathDiscoveryPoint[500];
        int counter = 0;
        for (float i = 0; i < 1.0f; i += NormalizedTLeaps)
        {
            PathDiscoveryPoint pd = PathDiscoveryPointPool.Get();
            pd.transform.position = spline.GetPoint(i);
            pd.Proximity = i;
            //pd.gameObject.SetActive(true);
            pds[counter] = pd;
            pd = null;
            counter += 1;
        }

        pathphobject = GameObjectPool.Instance.PlaceHoldersDict["_PlaceHolder_" + PathDiscoveryPointPrefab.name];
        
        

        StartCoroutine(movePathObjectsAndTriggerDetection(pds));
    }
}
