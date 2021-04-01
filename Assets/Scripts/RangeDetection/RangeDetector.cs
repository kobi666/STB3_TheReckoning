using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class RangeDetector : TagDetector,IQueueable<RangeDetector>
{
    [ColorPalette] public Color Color = Color.red;


    private RangeDebug _rangeDebug;
    float rangeRadius = 1;
    public float RangeRadius {
        get => rangeRadius ;
        set {
            rangeRadius = value;
        } 
    }
    
    
    Sprite sprite;
    public Type QueueableType {get;set;}

    public void OnEnqueue() {

    }

    public void OnDequeue()
    {
        
    }

    PoolObjectQueue<RangeDetector> queuePool;
    public PoolObjectQueue<RangeDetector> QueuePool { get;set;}
    public CircleCollider2D RangeCollider;
    
/*
#if UNITY_EDITOR
    [TagSelectorAttribute][SerializeField]
# endif
    public string[] DiscoverableTags = new string[] { };
     public void AddTagToDiscoverableTags(string _tag) {
        
        for (int i = 0; i < DiscoverableTags.Length ; i++)
        {
            if (String.IsNullOrEmpty(DiscoverableTags[i])) {
                DiscoverableTags[i] = _tag;
                break;
            }
            if (i >= DiscoverableTags.Length -1) {
                Debug.LogWarning("No Empty Tag slots!");
            }
        }
     }
     List<string> DiscoverableTagsList = new List<string>();
    // Start is called before the first frame update
    public event Action<GameObject> onTargetEnter;
    public void OnTargetEnter(GameObject target) {
        onTargetEnter?.Invoke(target);
    }

    public event Action<string,string> onTargetExit;
    public void OnTargetExit(string targetName, string callerName) {
        onTargetExit?.Invoke(targetName, callerName);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (DiscoverableTagsList.Contains(other.tag)) {
            OnTargetEnter(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (DiscoverableTagsList.Contains(other.tag)) {
            OnTargetExit(other.name, name);
        }
    }
    */

private void OnDrawGizmos()
{
    Gizmos.color = Color;
}

private void OnDrawGizmosSelected()
{
    Gizmos.color = Color;
}

public override bool IsPositionInRange(Vector2 pos)
{
    Vector2 self = transform.position;
    if ((self - pos).sqrMagnitude < rangeRadius * rangeRadius)
    {
        return true;
    }

    return false;
}

void Awake()
    {
        
        base.Awake();
        RangeCollider = GetComponent<CircleCollider2D>() ?? null;
        OnDrawGizmosSelected();
        OnDrawGizmos();
    }

    public override float GetSize()
    {
        return RangeCollider.radius;
    }
    
    
    
    public override void UpdateSize(float newSize) {
        if (RangeCollider == null) {
            RangeCollider = gameObject.GetComponent<CircleCollider2D>();
        }

        RangeCollider.radius = newSize;
        if (_rangeDebug == null)
        {
            _rangeDebug = GetComponentInChildren<RangeDebug>() ?? null;
        }
        if (_rangeDebug?.gameObject.activeSelf == true) {
        _rangeDebug.transform.localScale = new Vector3(newSize,newSize,newSize);
        }

        RangeRadius = newSize;
    }

    
    void Start()
    {
        RangeCollider = gameObject.GetComponent<CircleCollider2D>() ?? null;
        _rangeDebug = GetComponentInChildren<RangeDebug>() ?? null;
    }

    
    void OnDisable()
    {
        QueuePool?.ObjectQueue.Enqueue(this);
    }

    
    


}
