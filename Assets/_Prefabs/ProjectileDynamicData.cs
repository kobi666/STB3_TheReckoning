using UnityEngine;

public class ProjectileDynamicData
{
    public Effectable EffectableTarget = null;
    public Vector2 TargetPosition;
    static Vector2 nowhere = new Vector2(9999,9999);
    public void Clear()
    {
        if (EffectableTarget != null) EffectableTarget = null;
        TargetPosition = nowhere;
    }
}
