using UnityEngine;

public class PathDiscoveryTargetBank : TargetBank<PathDiscoveryPoint>
{
    public override PathDiscoveryPoint TryToGetTargetOfType(int gameObjectID)
    {
        PathDiscoveryPoint pd = null;
        if (GameObjectPool.Instance.ActivePathDiscoveryPoints.Contains(gameObjectID)) {
            pd = GameObjectPool.Instance.ActivePathDiscoveryPoints.Pool[gameObjectID] ?? null;
        }
        return pd;
    }

    public override void PostStart()
    {
        
    }
}
