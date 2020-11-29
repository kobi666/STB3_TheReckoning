using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

public class AreaEffect : SerializedMonoBehaviour,IQueueable<AreaEffect>
{
	// Start is called before the first frame update
	public EffectableTargetBank TargetBank;
	protected Dictionary<string,Effectable> Targets
	{
		get => TargetBank.Targets;
	}

	public void ApplyEffect(Effect effect)
	{
		foreach (var target in Targets.Values)
		{
			if (target != null)
			{
				effect.Apply(target);
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
}
