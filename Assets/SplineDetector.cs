using System;
using UnityEngine;

public class SplineDetector : TagDetector
{
    

    public override bool IsPositionInRange(Vector2 pos)
    {
        throw new NotImplementedException("Not implamented in SplineDetector Yet");
    }

    public override float GetSize()
    {
        return 9999f;
    }

    public override void UpdateSize(float newSize)
    {
        
    }
}
