using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using MyBox;
using UnityEditor;
using Random = UnityEngine.Random;

public class PathPointFinder : MonoBehaviour
{
#if UNITY_EDITOR
    [TagSelector][SerializeField]
# endif
    public string splineTag;
    
    [Required]
    public CollisionDetector RangeDetector;

    public bool OnlyTargetSpecificPaths = false;
    [ShowIf("OnlyTargetSpecificPaths")][ValidateInput("specificPathsNotEmpty", "List Cannot Be Empty")]
    public List<PathController> SpecifiedPaths;

    bool specificPathsNotEmpty()
    {
        return !SpecifiedPaths.IsNullOrEmpty();
    }
    
    [Required]
    public IHasRangeComponents ParentRangeController;
    
    [Serializable]
    public enum PathPointType
    {
        None,
        MiddlePoint,
        ClosestToEnd,
        ClosestToStart,
        SetPosition,
        RandomPosition
    }

    public PathPointType[] pathPointTypePriority;

    public Vector2? GetPathPointByPriority()
    {
        Vector2? placeholderV2 = null;
        if (!pathPointTypePriority.IsNullOrEmpty()) {
            foreach (var ppp in pathPointTypePriority)
        {
            if (ppp == PathPointType.MiddlePoint)
            {
                placeholderV2 = FindMiddlePoint();
                if (placeholderV2 != null)
                {
                    return placeholderV2;
                }
            }

            if (ppp == PathPointType.ClosestToEnd)
            {
                placeholderV2 = FindClosestPointToEndOfSpline();
                if (placeholderV2 != null)
                {
                    return placeholderV2;
                }
            }
            if (ppp == PathPointType.ClosestToStart)
            {
                placeholderV2 = FindClosestToStart();
                if (placeholderV2 != null)
                {
                    return placeholderV2;
                }
            }

            if (ppp == PathPointType.RandomPosition)
            {
                placeholderV2 = new Vector2(Random.Range(-100f,100f),Random.Range(-100f,100f));
                return placeholderV2;
            }
            
        }
            
        }

        return placeholderV2;
    }
    
    public Vector2? GetPathPointByPriority(PathPointType[] pointTypePriorty)
    {
        Vector2? placeholderV2 = null;
        if (!pathPointTypePriority.IsNullOrEmpty()) {
            foreach (var ppp in pointTypePriorty)
            {
                if (ppp == PathPointType.MiddlePoint)
                {
                    placeholderV2 = FindMiddlePoint();
                    if (placeholderV2 != null)
                    {
                        return placeholderV2;
                    }
                }

                if (ppp == PathPointType.ClosestToEnd)
                {
                    placeholderV2 = FindClosestPointToEndOfSpline();
                    if (placeholderV2 != null)
                    {
                        return placeholderV2;
                    }
                }
                if (ppp == PathPointType.ClosestToStart)
                {
                    placeholderV2 = FindClosestToStart();
                    if (placeholderV2 != null)
                    {
                        return placeholderV2;
                    }
                }
                if (ppp == PathPointType.RandomPosition)
                {
                    placeholderV2 = new Vector2(Random.Range(-100f,100f),Random.Range(-100f,100f));
                    return placeholderV2;
                }
            
            }
            
        }

        return placeholderV2;
    }

    

    public Vector2? FindMiddlePoint()
    {
        Vector2? v2 = null;
        if (!PathSplines.IsNullOrEmpty()) {
            SplinePathController spc = PathSplines[FindShortestSpline()];
            SortedList<int,Vector2> points = new SortedList<int, Vector2>();
            int counter = 0;
            foreach (var sp in spc.splinePoints)
            {
                if (RangeDetector.IsPositionInRange(sp.Value))
                {
                    points.Add(counter,sp.Value);
                    counter++;
                }
            }
            if (!points.IsNullOrEmpty()) {
           v2 = points[points.Count / 2];
            }
        }

        return v2;
    }

    public Vector2? FindClosestToStart()
    {
        Vector2? v2p = null;
        shortestSplinePathGID = FindShortestSpline();
        SplinePathController spc = PathSplines[shortestSplinePathGID];
        foreach (var v2 in spc.splinePoints)
        {
            if (RangeDetector.IsPositionInRange(v2.Value))
            {
                return v2.Value;
            }
        }

        return v2p;
    }
    

    [ShowInInspector]
    public Dictionary<int,SplinePathController> PathSplines = new Dictionary<int, SplinePathController>();


    private List<string> specificPathNames = new List<string>();
    public event Action onPathFound;
    
    public void OnPathFound()
    {
        onPathFound?.Invoke();
    }
    void AddPathSplines(int targetGameObjectCollisionID)
    {
        int targetGID = GameObjectPool.CollisionIDToGameObjectID[targetGameObjectCollisionID].Item1;
        if (GameObjectPool.Instance.ActiveSplines.Contains(targetGID))
            {
                if (OnlyTargetSpecificPaths)
                {
                    if (specificPathNames.IsNullOrEmpty())
                    {
                        specificPathNames.Clear();
                        foreach (var specificPath in SpecifiedPaths)
                        {
                            specificPathNames.Add(specificPath.name);
                        }
                    }

                    if (!specificPathNames.Contains(GameObjectPool.Instance.ActiveSplines.Pool[targetGID].parentPath.name))
                    {
                        return;
                    }
                }
                if (GameObjectPool.Instance.ActiveSplines.Pool[targetGID].SplineType == SplineTypes.Main_Middle) {
                    {
                        if (!PathSplines.ContainsKey(targetGID)) {
                            PathSplines.Add(targetGID,GameObjectPool.Instance.ActiveSplines.Pool[targetGID]);
                            onPathFound?.Invoke();    
                            }
                        }
                }
            }
        
    }

    private int shortestSplinePathGID;
    public int  FindShortestSpline()
    {
        int sgid = 0;
        if (!PathSplines.IsNullOrEmpty())
        {
            float length = 999;
            
            foreach (var ps in PathSplines)
            {
                float l = ps.Value.BgCcMath.GetDistance();
                if (l < length)
                {
                    length = l;
                    sgid = ps.Key;
                }
            }
        }

        return sgid;
    }
    
    public Vector2? FindClosestPointToEndOfSpline()
    {
        shortestSplinePathGID = FindShortestSpline();
        
        SplinePathController spc = PathSplines[shortestSplinePathGID];
        SortedList<int, Vector2> splinePoints = PathSplines[shortestSplinePathGID].splinePoints;
        float closenss = 0f;
        Vector2? tv2 = null;
        foreach (var v2 in splinePoints)
        {
            if (RangeDetector.IsPositionInRange(v2.Value))
            {
                
                float c = spc.GetPointIndexAccordingToRation(v2.Key);
                string s;
                if (spc.GetPointIndexAccordingToRation(v2.Key) > closenss)
                {
                    tv2 = v2.Value;
                    closenss = spc.GetPointIndexAccordingToRation(v2.Key);
                }
            }
        }
        return tv2;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        RangeDetector.onTargetEnter += AddPathSplines;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
