using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class CollidingObject : MonoBehaviour
{
    public Color BoxColor = Color.white;
    
    public int CollisionID;
    public abstract DetectionTags CollisionTag { get; }
    public abstract List<DetectionTags> TagsICanDetect { get; }
    public BoxCollider2D BoxCollider2D;
    
    public int CollisionTagInt;
    public int CollisionTagsICanDetectInt;

    protected void Awake()
    {
        CollisionID = IDGenerator.GetID();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        
        if (CollisionTag == DetectionTags.NONE)
        {
            CollisionTagInt = 0;
        }
        else
        {
            CollisionTagInt = 1<<(int) CollisionTag;    
        }
        
        CollisionTagsICanDetectInt = ConvertDetetableTypesToInt(TagsICanDetect);
    }
    
    void OnDrawGizmos()
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
    
    

    protected void Start()
    {
        BoxCollider2D.enabled = false;
        GWCS.instance.AddObject(this);
    }
    
    
    
    int ConvertDetetableTypesToInt(List<DetectionTags> detectionTypes)
    {
        int convertedInt = 0;
        
        foreach (var detectionType in detectionTypes)
        {
            if (detectionType == DetectionTags.NONE)
            {
                if (detectionTypes.Count > 1)
                {
                    throw new Exception("None detected with other Detectable Types");
                }
                return 0;
            }
            convertedInt |= 1 << (int) detectionType;
        }

        
        return convertedInt;
    }
}
