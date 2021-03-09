using System.Collections.Generic;

public interface BuffNerf
{
    
}


public interface IBuffNerfAble
{
    void ApplyEffectBuffs(List<BuffNerf> buffnerfs);
    
}
