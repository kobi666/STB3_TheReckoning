using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class test3 : SerializedMonoBehaviour
{
    public Transform target;
    public float duration;
    private RangeDetector rd;
    

    private void Start()
    {
        rd = GetComponentInChildren<RangeDetector>();
    }
}
