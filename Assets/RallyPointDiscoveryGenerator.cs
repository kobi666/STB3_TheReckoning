using System;
using System.Collections;
using System.Collections.Generic;
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

    protected void Start()
    {
        if (DistanceBetweenPoints <= 0)
        {
            DistanceBetweenPoints = 0.3f;
        }
        OnPathCalculation();
        PathDiscoveryPointPool = GameObjectPool.Instance.GetPathDiscoveryPool(PathDiscoveryPointPrefab);
        for (float i = 0; i < 1.0f; i += NormalizedTLeaps)
        {
            PathDiscoveryPoint pd = PathDiscoveryPointPool.GetInactive();
            pd.transform.position = spline.GetPoint(i);
            pd.gameObject.SetActive(true);
        }
    }
}
