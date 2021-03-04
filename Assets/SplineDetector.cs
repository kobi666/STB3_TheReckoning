using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class SplineDetector : TagDetector
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    public override bool IsPositionInRange(Vector2 pos)
    {
        throw new NotImplementedException("Not implamented in SplineDetector Yet");
    }

    public override float GetSize()
    {
        return 9999f;
    }

    public override void UpdateSize(float size)
    {
        
    }
}
