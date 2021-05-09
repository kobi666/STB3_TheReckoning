using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CollisionDetector : CollidingObject
{
    public float ColliderSize;
    private Bounds ColliderBounds;
    public Color BoxColor = Color.white;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = BoxColor;
        if (BoxCollider2D != null)
        {
            Vector2 Pos = (Vector2)transform.position + BoxCollider2D.offset;
            Vector3 Size = new Vector3(BoxCollider2D.size.x, BoxCollider2D.size.y, 1f);
            Size /= 2f;

            Vector2[] Points = new Vector2[4];
            Points[0].Set(Pos.x - Size.x, Pos.y + Size.y);
            Points[1].Set(Pos.x + Size.x, Pos.y + Size.y);
            Points[2].Set(Pos.x + Size.x, Pos.y - Size.y);
            Points[3].Set(Pos.x - Size.x, Pos.y - Size.y);

            float RotationInRad = transform.eulerAngles.z * Mathf.Deg2Rad;

            for(int i=0; i<4; i++)
            {
                float x = Points[i].x - Pos.x, y = Points[i].y - Pos.y;

                Points[i].x = x * Mathf.Cos(RotationInRad) - y * Mathf.Sin(RotationInRad);
                Points[i].y = x * Mathf.Sin(RotationInRad) + y * Mathf.Cos(RotationInRad);

                Points[i].x = Points[i].x + Pos.x;
                Points[i].y = Points[i].y + Pos.y;
            }                   

            Gizmos.DrawLine(Points[0], Points[1]);
            Gizmos.DrawLine(Points[1], Points[2]);
            Gizmos.DrawLine(Points[2], Points[3]);
            Gizmos.DrawLine(Points[3], Points[0]);
        }
    }
    
    
    public float GetSize()
    {
        return ColliderSize;
    }

    public void UpdateSize(float newSize)
    {
        var baseSize = BoxCollider2D.size;
        baseSize = new Vector2( newSize, newSize);
        BoxCollider2D.size = baseSize;
        ColliderSize = newSize;
        if (newSize > GWCS.instance.QuadrentCellSize)
        {
            Debug.LogError("Collider size bigger than Quadrant Cell size!");
        }
        ColliderBounds = BoxCollider2D.bounds;
    }
    
    // Start is called before the first frame update
    public event Action<int> onTargetEnter;
    public void OnTargetEnter(int collisionID) {
        onTargetEnter?.Invoke(collisionID);
    }

    public event Action<int,string> onTargetExit;
    public void OnTargetExit(int collisionID,string callerName) {
        onTargetExit?.Invoke(collisionID,callerName);
    }

    public bool IsPositionInRange(Vector2 pos)
    {
        Vector2 selfPos = (Vector2)transform.position + BoxCollider2D.offset;
        if (pos.x > selfPos.x - BoxCollider2D.size.x && pos.x < selfPos.x + BoxCollider2D.size.x)
        {
            if (pos.y > selfPos.y - BoxCollider2D.size.y && pos.y < selfPos.x + BoxCollider2D.size.y)
            {
                return true;
            }
        }

        return false;
    }

    public List<DetectionTags> tagsICanDetect = new List<DetectionTags>();
    public override DetectionTags CollisionTag
    {
        get => DetectionTags.NONE;
        set
        {
            
        }
    }
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }
}
