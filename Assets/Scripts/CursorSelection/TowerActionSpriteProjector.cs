using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerActionSpriteProjector : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    public Sprite ActionSprite;
    
    [Required]
    public Sprite DefaultSprite;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
