using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public abstract class TagDetector : MonoBehaviour
{
#if UNITY_EDITOR
    [TagSelector][SerializeField]
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
    public event Action<GameObject,string> onTargetEnter;
    public void OnTargetEnter(GameObject target, string _tag) {
        onTargetEnter?.Invoke(target, _tag);
    }

    public event Action<string,string> onTargetExit;
    public void OnTargetExit(string targetName, string callerName) {
        onTargetExit?.Invoke(targetName, callerName);
    }

    public abstract float GetSize();
    public abstract void UpdateSize(float size);

    private ContactPoint2D[] cp;
    protected void OnTriggerEnter2D(Collider2D other) {
        OnTargetEnter(other.gameObject, other.tag);
            //var t = other.
    }

    public abstract bool IsPositionInRange(Vector2 pos);

    private void OnTriggerExit2D(Collider2D other) {
        if (DiscoverableTagsList.Contains(other.tag)) {
            OnTargetExit(other.name, name);
        }
    }

    protected void Awake()
    {
        foreach (string item in DiscoverableTags)
        {
            if (!String.IsNullOrEmpty(item)) {
                DiscoverableTagsList.Add(item);
            }
        }
    }
}
