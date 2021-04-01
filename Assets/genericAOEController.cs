using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GenericAOEController : MonoBehaviour
{
    [Required]
    public CollisionDetector Detector;

    public int SingelTargetID;
    public event Action<int> onSingleTargetSet;

    public void OnSingleTargetSet(int singleTargetID)
    {
        onSingleTargetSet?.Invoke(singleTargetID);
    }

    public void OnSingleTargetClear()
    {
        OnSingleTargetSet(0);
    }
    
    [ShowInInspector]
    public ITargeter parentTargeter;
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
        TargetBank = TargetBank ?? GetComponent<EffectableTargetBank>();
        parentTargeter = transform.parent.GetComponent<ITargeter>();
        onSingleTargetSet += delegate(int s) { SingelTargetID = s; };
        parentTargeter.onTargetSet += onSingleTargetSet.Invoke;
    }
}
