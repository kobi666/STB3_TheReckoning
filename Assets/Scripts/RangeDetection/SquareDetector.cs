using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareDetector : TagDetector
{

    public BoxCollider2D BoxCollider;
    
    public override float GetSize()
    {
        return BoxCollider.size.sqrMagnitude;
    }

    public override void UpdateSize(float SizeDelta)
    {
        
    }

    void Awake()
    {
        base.Awake();
        BoxCollider = BoxCollider ?? GetComponent<BoxCollider2D>();
    }
}
