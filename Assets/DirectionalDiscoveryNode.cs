using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DirectionalDiscoveryNode : MonoBehaviour
{
    [Required]
    public BoxCollider2D MyBoxCollider2D;
    [Required]
    public RectTransform MyRectTransform;

    public bool _DEBUG;

    private void Awake()
    {
        MyRectTransform.sizeDelta = new Vector2(MyBoxCollider2D.size.x,MyBoxCollider2D.size.y);
    }
}
