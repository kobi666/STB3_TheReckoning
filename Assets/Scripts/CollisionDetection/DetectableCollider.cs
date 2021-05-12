using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DetectableCollider : CollidingObject
{
    public DetectionTags collisionTag;
    public override bool RegisterToGWCS { get => _registerToGwcs ; set => _registerToGwcs = value; }
    private bool _registerToGwcs = true;
    public override DetectionTags CollisionTag { get => collisionTag;
        set => collisionTag = value;
    }
    public override List<DetectionTags> TagsICanDetect { get => new List<DetectionTags>(); }
    public Color BoxColor = Color.red;
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
}
