using UnityEngine;

public class PathDiscoveryTargetBank : TargetBank<PathDiscoveryPoint>
{
    public override PathDiscoveryPoint TryToGetTargetOfType(GameObject go)
    {
        PathDiscoveryPoint pd = null;
        if (GameObjectPool.Instance.ActivePathDiscoveryPoints.Contains(go.name)) {
            pd = GameObjectPool.Instance.ActivePathDiscoveryPoints.Pool[go.name] ?? null;
        }
        return pd;
    }

    public override void PostStart()
    {
        
    }
}
