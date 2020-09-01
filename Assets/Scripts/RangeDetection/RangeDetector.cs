using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeDetector : MonoBehaviour,IQueueable<RangeDetector>
{
    private RangeDebug _rangeDebug;
    float rangeRadius = 1;
    public float RangeRadius {
        get => rangeRadius ;
        set {
            rangeRadius = value;
            SetRangeRadius(value);
        } 
    }
    
    
    Sprite sprite;
    public Type QueueableType {get;set;}

    public void OnEnqueue() {

    }
    PoolObjectQueue<RangeDetector> queuePool;
    public PoolObjectQueue<RangeDetector> QueuePool { get;set;}
    public CircleCollider2D RangeCollider;

    [TagSelector]
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

    void Awake()
    {
        RangeCollider = GetComponent<CircleCollider2D>() ?? null;
        foreach (string item in DiscoverableTags)
        {
            if (!String.IsNullOrEmpty(item)) {
            DiscoverableTagsList.Add(item);
            }
        }
    }

    public void SetRangeRadius(float range) {
        if (RangeCollider == null) {
            RangeCollider = gameObject.GetComponent<CircleCollider2D>();
        }

        RangeCollider.radius = range;
        if (_rangeDebug == null)
        {
            _rangeDebug = GetComponentInChildren<RangeDebug>() ?? null;
        }
        if (_rangeDebug?.gameObject.activeSelf == true) {
        _rangeDebug.transform.localScale = new Vector3(range,range,range);
        }
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
