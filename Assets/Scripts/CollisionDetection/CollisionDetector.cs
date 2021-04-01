using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CollisionDetector : CollidingObject
{
    private float ColliderSize;
    private Bounds ColliderBounds;
    
    public float GetSize()
    {
        return ColliderSize;
    }

    public void UpdateSize(float newSize)
    {
        var baseSize = BoxCollider2D.size;
        baseSize = new Vector2(baseSize.x * newSize,baseSize.y * newSize);
        BoxCollider2D.size = baseSize;
        ColliderSize = newSize;
        ColliderBounds = BoxCollider2D.bounds;
    }
    
    // Start is called before the first frame update
    public event Action<int> onTargetEnter;
    public void OnTargetEnter(int collisionID) {
        onTargetEnter?.Invoke(collisionID);
    }

    public event Action<int> onTargetExit;
    public void OnTargetExit(int collisionID) {
        onTargetExit?.Invoke(collisionID);
    }

    public bool IsPositionInRange(Vector2 pos)
    {
        if (pos.x > ColliderBounds.min.x && pos.x < ColliderBounds.max.x)
        {
            if (pos.y > ColliderBounds.min.y && pos.y < ColliderBounds.max.y)
            {
                return true;
            }
        }

        return false;
    }
    
    
    public override DetectionTags CollisionTag { get => DetectionTags.NONE; }
    
    public List<DetectionTags> tagsICanDetect = new List<DetectionTags>();
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }

    protected void Awake()
    {
        base.Awake();
    }
}
