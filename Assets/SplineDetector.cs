using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class SplineDetector : TagDetector
{
    private ContactPoint2D[] cpt = new ContactPoint2D[1];
    private void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    public override float GetSize()
    {
        return 9999f;
    }

    public override void UpdateSize(float newSize)
    {
        
    }
}
