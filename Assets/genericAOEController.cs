using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GenericAOEController : MonoBehaviour
{
    [Required]
    public TagDetector Detector;

    public string SingleTargetName;
    public event Action<string> onSingleTargetSet;

    public void OnSingleTargetSet(string singleTargetName)
    {
        onSingleTargetSet?.Invoke(singleTargetName);
    }

    public void OnSingleTargetClear()
    {
        OnSingleTargetSet(string.Empty);
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
        Detector = Detector ?? GetComponentInChildren<TagDetector>();
        TargetBank = TargetBank ?? GetComponent<EffectableTargetBank>();
        parentTargeter = transform.parent.GetComponent<ITargeter>();
        onSingleTargetSet += delegate(string s) { SingleTargetName = s; };
        parentTargeter.onTargetSet += onSingleTargetSet.Invoke;
    }
}
