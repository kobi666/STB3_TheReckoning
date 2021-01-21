using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BuffNerf
{
    
}


public interface IBuffNerfAble
{
    void ApplyEffectBuffs(List<BuffNerf> buffnerfs);
    
}
