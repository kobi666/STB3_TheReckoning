using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

public class GenericAOEController : SerializedMonoBehaviour
{
    public TagDetector Detector;
    private float range;
    public float Range
    {
        get => range;
        set
        {
            Detector.SetSize(value);
            range = value;
        }
    }
    public EffectableTargetBank TargetBank;

    protected void Awake()
    {
        Detector = Detector ?? GetComponentInChildren<TagDetector>();
        TargetBank = TargetBank ?? GetComponent<EffectableTargetBank>();
    }
}
