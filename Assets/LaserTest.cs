using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest : MonoBehaviour
{
    private ProjectileExitPoint pep;
    Transform pepTransform;
    public List<Transform> LaserPoints;
    public Transform[] LaserPointsArrayWithOrigin;

    private LineRenderer _lineRenderer;
    
    //public void MoveLineInArc
    
    
    void Start()
    {
        pep = GetComponentInChildren<ProjectileExitPoint>();
        pepTransform = pep.transform;
        
        LaserPointsArrayWithOrigin = new Transform[LaserPoints.Count + 1];
        for (int i = 0; i < LaserPoints.Count; i++)
        {
            if (i == 0)
            {
                LaserPointsArrayWithOrigin[0] = pepTransform;
            }

            LaserPointsArrayWithOrigin[i + 1] = LaserPoints[i];
        }
        
        
        _lineRenderer = GetComponent<LineRenderer>();
        //_lineRenderer.enabled = false;
        _lineRenderer.useWorldSpace = true;
        
    }
    
    
    
    void Update()
    {
        _lineRenderer.positionCount = LaserPointsArrayWithOrigin.Length;
        for (int i = 0; i < LaserPointsArrayWithOrigin.Length; i++)
        {
            _lineRenderer.SetPosition(i, LaserPointsArrayWithOrigin[i].position);
        }
    }
}
