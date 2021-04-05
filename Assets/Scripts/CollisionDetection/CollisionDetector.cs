using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CollisionDetector : CollidingObject
{
    private float ColliderSize;
    private Bounds ColliderBounds;
    
    public Color BoxColor = Color.white;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = BoxColor;
        if (BoxCollider2D != null)
        {
            Vector3 Pos = transform.position;
            Vector3 Size = new Vector3(BoxCollider2D.size.x * transform.lossyScale.x, BoxCollider2D.size.y * transform.lossyScale.y, 1f);
            Size /= 2f;

            Vector3[] Points = new Vector3[4];
            Points[0].Set(Pos.x - Size.x, Pos.y + Size.y, Pos.z);
            Points[1].Set(Pos.x + Size.x, Pos.y + Size.y, Pos.z);
            Points[2].Set(Pos.x + Size.x, Pos.y - Size.y, Pos.z);
            Points[3].Set(Pos.x - Size.x, Pos.y - Size.y, Pos.z);

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
        if (pos.x > ColliderBounds.min.x && pos.x < ColliderBounds.max.x)
        {
            if (pos.y > ColliderBounds.min.y && pos.y < ColliderBounds.max.y)
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
    }
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }
}
