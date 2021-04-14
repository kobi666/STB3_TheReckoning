using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollisionDetector : CollisionDetector
{
    public override DetectionTags CollisionTag { get => DetectionTags.NONE; }
    
    
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }
    public CollisionAggregator ParentCollisionAggregator;

    public void InitChildDetector(MyGameObject parentMyGameObject, CollisionAggregator parentCollisionAggregator,
        List<DetectionTags> detectionTags)
    {
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
