using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class ChildCollisionDetector : CollisionDetector
{
    public override DetectionTags CollisionTag { get => DetectionTags.NONE;
        set
        {
            
        }
    }
    public DetectableCollider DetectableCollider;
    
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }
    public CollisionAggregator ParentCollisionAggregator;

    public void InitChildDetector(MyGameObject parentMyGameObject, CollisionAggregator parentCollisionAggregator,
        List<DetectionTags> detectionTags, DetectionTags myDetectionTag)
    {
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
    }

    protected void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        base.Start();
        onTargetEnter += ParentCollisionAggregator.OnChildDetectorAdd;
        onTargetExit += ParentCollisionAggregator.OnChildDetectorRemove;
    }
}
