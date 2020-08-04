using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class RangeDetector : MonoBehaviour
{
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

    public event Action<string> onTargetExit;
    public void OnTargetExit(string targetName) {
        onTargetExit?.Invoke(targetName);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (DiscoverableTagsList.Contains(other.tag)) {
            OnTargetEnter(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (DiscoverableTagsList.Contains(other.tag)) {
            OnTargetExit(other.name);
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


}
