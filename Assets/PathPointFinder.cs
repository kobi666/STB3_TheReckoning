using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using MyBox;
using UnityEditor;

public class PathPointFinder : MonoBehaviour
{
#if UNITY_EDITOR
    [TagSelector][SerializeField]
# endif
    public string splineTag;
    
    [Required]
    public RangeDetector RangeDetector;
    
    [Required]
    public IHasRangeComponents ParentRangeController;


    private void Awake()
    {
        
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
            v2 = points[points.Count / 2];
        }

        return v2;

    }


    [ShowInInspector]
    public Dictionary<string,SplinePathController> PathSplines = new Dictionary<string, SplinePathController>();
    
    public event Action onPathFound;
    void AddPathSplines(GameObject go, string _tag)
    {
        if (_tag == splineTag) {
            if (GameObjectPool.Instance.ActiveSplines.Contains(go.name))
            {
                if (GameObjectPool.Instance.ActiveSplines.Pool[go.name].SplineType == SplineTypes.Main) {
                    try
                    {
                        PathSplines.Add(go.name,GameObjectPool.Instance.ActiveSplines.Pool[go.name]);
                        onPathFound?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                }
            }
        }
    }

    private string shortestSplinePath;
    string  FindShortestSpline()
    {
        string sname = string.Empty;
        if (!PathSplines.IsNullOrEmpty())
        {
            float length = 999;
            
            foreach (var ps in PathSplines)
            {
                float l = ps.Value.BgCcMath.GetDistance();
                if (l < length)
                {
                    length = l;
                    sname = ps.Key;
                }
            }
        }

        return sname;
    }
    
    public Vector2? FindClosestPointToEndOfSpline()
    {
        shortestSplinePath = FindShortestSpline();
        
        SplinePathController spc = PathSplines[shortestSplinePath];
        SortedList<int, Vector2> splinePoints = PathSplines[shortestSplinePath].splinePoints;
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
        RangeDetector.gameObject.SetActive(false);
        RangeDetector.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
