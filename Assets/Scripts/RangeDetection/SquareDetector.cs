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

    public override void UpdateSize(float size)
    {
        BoxCollider.size = new Vector2(size,size);
        ColliderSize = size;
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
