using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableCollider : CollidingObject
{
    public DetectionTags collisionTag;
    public override DetectionTags CollisionTag { get => collisionTag; }
    public override List<DetectionTags> TagsICanDetect { get => new List<DetectionTags>(); }
}
