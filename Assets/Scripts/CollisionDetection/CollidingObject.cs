using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public abstract class CollidingObject : MonoBehaviour
{
    [Required][SerializeField] public MyGameObject ParentMyGameObject;
    
    public bool FirstRun = true;
    public abstract bool RegisterToGWCS { get; set; }
    protected void OnEnable()
    {
        if (RegisterToGWCS) {
        GWCS.instance.AddObject(this, CollisionID);
        }
    }

    protected void OnDisable()
    {
        if (RegisterToGWCS)
        {
            GWCS.instance.RemoveObject(this);
        }
    }

    public int GameObjectID
    {
        get => ParentMyGameObject.MyGameObjectID;
    }
    
    public int CollisionID;
    
    public abstract DetectionTags CollisionTag { get; set; }
    public abstract List<DetectionTags> TagsICanDetect { get; }
    public BoxCollider2D BoxCollider2D;
    
    public int CollisionTagInt;
    public int CollisionTagsICanDetectInt;


    protected void Awake()
    {
        if (RegisterToGWCS)
        {
            CollisionID = IDGenerator.Instance.GetCollisionID();
            BoxCollider2D = GetComponent<BoxCollider2D>();

            if (CollisionTag == DetectionTags.NONE)
            {
                CollisionTagInt = 0;
            }
            else
            {
                CollisionTagInt = 1 << (int) CollisionTag;
            }

            CollisionTagsICanDetectInt = ConvertDetetableTypesToInt(TagsICanDetect);

            BoxCollider2D.enabled = false;
        }
    }
    
    


    protected void Start()
    {
        if (RegisterToGWCS)
        {
            GameObjectPool.CollisionIDToGameObjectID.TryAdd(CollisionID, (GameObjectID, name));
        }
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
