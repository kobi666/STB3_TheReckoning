using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;

[RequireComponent(typeof(LineRenderer))]
public class RangeDrawer : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public float LineWidth;
    Vector3[] points = new Vector3[361];
    private float parentZ;
    private void Awake()
    {
        parentZ = transform.parent.transform.position.z;
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.startWidth = LineWidth;
        LineRenderer.endWidth = LineWidth;
        LineRenderer.useWorldSpace = false;
        LineRenderer.positionCount = points.Length;
    }

    
    
    public void DrawCircle(float radius)
    {
        if (radius <= 0)
        {
            LineRenderer.enabled = false;
        }
        
        else
        {
            LineRenderer.enabled = true;
            for (int i = 0; i < points.Length; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / 360);
                points[i] = new Vector3(Mathf.Sin(rad) * radius,Mathf.Cos(rad) * radius, parentZ -1 );
            }
            LineRenderer.SetPositions(points);
        }
    }

    
}
