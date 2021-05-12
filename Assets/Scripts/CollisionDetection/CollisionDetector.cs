using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CollisionDetector : CollidingObject
{

    public bool DebugCollision;
    public float ColliderSize;
    private Bounds ColliderBounds;
    public Color BoxColor = Color.white;
    void OnDrawGizmosSelected()
    {
        if (BoxCollider2D != null) {
        Gizmos.color = BoxColor;
        Gizmos.DrawWireCube(transform.position + (Vector3)BoxCollider2D.offset,new Vector3(BoxCollider2D.size.x,BoxCollider2D.size.y));
        }
    }    
    [Button]
    void DebugCollsion()
    {
        onTargetEnter += delegate(int i) { Debug.LogWarning("Found collision " + i + ", Object : " + GameObjectPool.CollisionIDToGameObjectID[i].Item2); };
        onTargetExit += delegate(int i, string s) { Debug.LogWarning("REMOVED collision " + i + ", Object : " + GameObjectPool.CollisionIDToGameObjectID[i].Item2); };
    }

    protected void Start()
    {
        base.Start();
    }

    protected void Awake()
    {
        base.Awake();
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

    public bool registerToGwcs = true;
    public override bool RegisterToGWCS { get => registerToGwcs; set => registerToGwcs = value; }

    public override DetectionTags CollisionTag
    {
        get => DetectionTags.NONE;
        set
        {
            
        }
    }
    public override List<DetectionTags> TagsICanDetect { get => tagsICanDetect; }
}
