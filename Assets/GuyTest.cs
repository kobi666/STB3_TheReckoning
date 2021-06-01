using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GuyTest : MonoBehaviour
{
    private RectTransform RectTransform;
    public Transform[] CornerTransforms = new Transform[4];

    [Button]
    void DebugIndicators()
    {
        for (int i = 0; i < CornerTransforms.Length; i++)
        {
            IndicatorsCornetPositions[i] = CornerTransforms[i].position;
        }
    }

    public Vector2[] IndicatorsCornetPositions = new Vector2[4]
    {
        Vector2.zero,
        Vector2.zero,
        Vector2.zero,
        Vector2.zero
    };
    
    [ShowInInspector]
    public Dictionary<CornerPositionNames, Vector2> CornerPositions = new Dictionary<CornerPositionNames, Vector2>();

    private float Xsize
    {
        get => BoxCollider2D.size.x * 0.5f;
    }

    private float YSize
    {
        get => BoxCollider2D.size.y * 0.5f;
    }
    public Vector3[] COrnerPositionsRectTransform = new Vector3[4];
    [Button]
    void GetCornerPositions()
    {
        RectTransform.GetWorldCorners(COrnerPositionsRectTransform);
        /*Vector2 phV2 = Vector2.zero;
        Vector2 position = transform.position;*/
    }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        /*CornerPositions.Add(CornerPositionNames.TopLeft,Vector2.zero);
        CornerPositions.Add(CornerPositionNames.TopRight,Vector2.zero);
        CornerPositions.Add(CornerPositionNames.BottomLeft,Vector2.zero);
        CornerPositions.Add(CornerPositionNames.BottomRight,Vector2.zero);*/
    }

    private BoxCollider2D BoxCollider2D;
    private void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }
}

public enum CornerPositionNames
{
    TopLeft,
    BottomLeft,
    TopRight,
    BottomRight
}


