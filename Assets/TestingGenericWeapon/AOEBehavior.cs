using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class AOEBehavior
{
    public List<GenericAOEController> AoeControllers = new List<GenericAOEController>();
    [OdinSerialize]
    public List<AOEEffect> Effects;

    public int DelayInAttackStartms = 5;
    public float AOESize = 1;
    public void initBehavior()
    {
        foreach (var aoeController in AoeControllers)
        {
            if (aoeController != null)
            {
                foreach (var aoeEffect in Effects)
                {
                    aoeEffect.InitEffect(AoeControllers);
                }
                aoeController.Detector.SetSize(AOESize);
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
