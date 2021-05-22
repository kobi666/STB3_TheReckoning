using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TowerActionSpriteProjector : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite ActionSprite;
    [ShowInInspector]
    private float ActionColorAlphaRO;

    public void SetSprite(TowerAction towerAction)
    {
        SpriteRenderer.sprite = towerAction.ActionSprite;
        var actionCOlor = towerAction.ActionColor;
        var newColor = new Color(actionCOlor.r,actionCOlor.g,actionCOlor.b,ActionColorAlphaRO);
    }

    public void DisableProjector()
    {
        SpriteRenderer.enabled = false;
        SpriteRenderer.sprite = null;
    }
    
    [Required]
    public Sprite DefaultSprite;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        ActionColorAlphaRO = SpriteRenderer.color.a;
    }
}
