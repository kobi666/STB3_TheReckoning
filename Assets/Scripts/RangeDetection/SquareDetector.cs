using UnityEngine;

public class SquareDetector : TagDetector
{
    public BoxCollider2D BoxCollider;
    private float ColliderSize;
    private Bounds ColliderBounds;
    
    public override float GetSize()
    {
        return ColliderSize;
    }

    public override void UpdateSize(float newSize)
    {
        var baseSize = BoxCollider.size;
        baseSize = new Vector2(baseSize.x * newSize,baseSize.y * newSize);
        BoxCollider.size = baseSize;
        ColliderSize = newSize;
        ColliderBounds = BoxCollider.bounds;
    }

    public override bool IsPositionInRange(Vector2 pos)
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

    void Awake()
    {
        base.Awake();
        BoxCollider = BoxCollider ?? GetComponent<BoxCollider2D>();
    }
}
