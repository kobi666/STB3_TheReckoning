using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CursorActionIndicator : MyGameObject
{
    [SerializeReference]
    public CursorActionBaseBase Action;
    [Required]
    public TowerActionSpriteProjector SpriteProjector;

    private float ActionColorAlpha;

    public Vector2 PositionDeltaFromCursor;
}
