using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class PathRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public BezierSpline spline;
 
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
 
    [ContextMenu( "Refresh" )]
    private void Update()
    {
        Synchronize( 5 );
    }
   
    public void Synchronize( int smoothness )
    {
        int numberOfPoints = spline.Count * Mathf.Clamp( smoothness, 1, 30 );
       
        Vector3[] positions = new Vector3[numberOfPoints + 1];
        float lineStep = 1f / numberOfPoints;
        for( int i = 0; i < numberOfPoints; i++ )
            positions[i] = spline.GetPoint( lineStep * i );
 
        positions[numberOfPoints] = spline.GetPoint( 1f );
 
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lineRenderer.loop = spline.loop;
    }
}
