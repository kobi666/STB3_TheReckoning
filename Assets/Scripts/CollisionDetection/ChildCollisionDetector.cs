using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class ChildCollisionDetector : CollisionDetector
{
    private bool _registerToGWCS = false;
    public override bool RegisterToGWCS { get => _registerToGWCS; set => _registerToGWCS = value; }

    public override DetectionTags CollisionTag { get => DetectionTags.NONE;
        set
        {
            
        }
    }
    public DetectableCollider DetectableCollider;
    
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }
    public CollisionAggregator ParentCollisionAggregator;

    public bool initialized = false;
    public void InitChildDetector(MyGameObject parentMyGameObject, CollisionAggregator parentCollisionAggregator,
        List<DetectionTags> detectionTags, DetectionTags myDetectionTag, int nameCounter)
    {
        name = "ChildCollisionDetector_" + nameCounter;
        if (myDetectionTag != DetectionTags.NONE)
        {
            DetectableCollider = gameObject.GetOrAddComponent<DetectableCollider>();
            DetectableCollider.ParentMyGameObject = parentMyGameObject;
            DetectableCollider.CollisionTag = myDetectionTag;
        }
        tagsICanDetect = detectionTags;
        ParentCollisionAggregator = parentCollisionAggregator;
        ParentMyGameObject = parentMyGameObject;
        RegisterToGWCS = true;
        onTargetEnter += ParentCollisionAggregator.OnChildDetectorAdd;
        onTargetExit += ParentCollisionAggregator.OnChildDetectorRemove;
        initialized = true;
        Awake();
        Start();
        GameObjectPool.CollisionIDToGameObjectID.TryAdd(CollisionID, (GameObjectID, name));
        GWCS.instance.AddObject(this, CollisionID);
    }

    
}
