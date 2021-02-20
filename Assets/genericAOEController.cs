using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class GenericAOEController : MonoBehaviour
{
    public TagDetector Detector;
    private float range;
    public float Range
    {
        get => range;
        set
        {
            Detector.UpdateSize(value);
            range = value;
        }
    }
    public EffectableTargetBank TargetBank;

    protected void Start()
    {
        Detector = Detector ?? GetComponentInChildren<TagDetector>();
        TargetBank = TargetBank ?? GetComponent<EffectableTargetBank>();
    }
}
