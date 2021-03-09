using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour,IQueueable<AreaEffect>
{
	// Start is called before the first frame update
	public EffectableTargetBank TargetBank;
	protected Dictionary<string,(Effectable,bool)> Targets
	{
		get => TargetBank.Targets;
	}

	public void ApplyEffect(Effect effect,Vector2 targetPos)
	{
		foreach (var target in Targets.Values)
		{
			if (target.Item1 != null)
			{
				effect.Apply(target.Item1, targetPos);
			} 
		}
	}

	public TagDetector Detector
	{
		get => TargetBank?.Detector;
	}

	public Type QueueableType { get => typeof(AreaEffect);
		set { }
	}
	public PoolObjectQueue<AreaEffect> QueuePool { get; set; } 
	public void OnEnqueue()
	{
		gameObject.SetActive(false);
	}

	public void OnDequeue()
	{
		
	}
}
