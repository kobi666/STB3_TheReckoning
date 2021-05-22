using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TowerActionIndicator : MyGameObject
{
    public TowerAction TowerAction;
    [Required]
    public TowerActionSpriteProjector SpriteProjector;

    private float ActionColorAlpha;

    public Vector2 PositionDeltaFromCursor;

    protected void Start()
    {
        
    }
}
