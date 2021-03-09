using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;


[Serializable]
public class AOEBehavior
{
    [Required]
    public List<GenericAOEController> AoeControllers = new List<GenericAOEController>();
    [SerializeField]
    public List<AOEEffect> Effects;

    public int DelayInAttackStartms = 5;
    public float AOESize = 1;
    public void initBehavior()
    {
        foreach (var aoeController in AoeControllers)
        {
            if (aoeController != null)
            {
                aoeController.Range = AOESize;
                foreach (var aoeEffect in Effects)
                {
                    aoeEffect.InitEffect(AoeControllers);
                }
                aoeController.Detector.UpdateSize(AOESize);
            }
        }
    }

    public void StartAOEBehavior()
    {
        foreach (var effect in Effects)
        {
            effect.OnAOEEffect();
        }
    }
    
}
