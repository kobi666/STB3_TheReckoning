using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class CollidingObject : MonoBehaviour
{
    [Required] public MyGameObject ParentMyGameObject;
    
    public int GameObjectID
    {
        get => ParentMyGameObject.MyGameObjectID;
    }
    
    public int CollisionID;
    
    public abstract DetectionTags CollisionTag { get; }
    public abstract List<DetectionTags> TagsICanDetect { get; }
    public BoxCollider2D BoxCollider2D;
    
    public int CollisionTagInt;
    public int CollisionTagsICanDetectInt;
    
    
    
    
    

    


    protected void Start()
    {
        CollisionID = IDGenerator.Instance.GetCollisionID();
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
        
        BoxCollider2D.enabled = false;
        GWCS.instance.AddObject(this);
        GameObjectPool.CollisionIDToGameObjectID.TryAdd(CollisionID,GameObjectID);
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
