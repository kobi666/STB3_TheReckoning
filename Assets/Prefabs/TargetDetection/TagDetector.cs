using System;
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

    public bool DebugTargetByTag;
    
#if UNITY_EDITOR
    [TagSelector]
# endif
    [SerializeField][ShowIf("DebugTargetByTag")]
    public string DebugTargetTag;

    void DebugTarget(GameObject go, string _tag)
    {
        if (_tag == DebugTargetTag) {
        Debug.LogError(go.name + " " + _tag);
        }
    }
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
    public event Action<MyGameObject,string> onTargetEnter;
    public void OnTargetEnter(MyGameObject target, string _tag) {
        onTargetEnter?.Invoke(target, _tag);
    }

    public event Action<int,string> onTargetExit;
    public void OnTargetExit(int gameObjectID, string callerName) {
        onTargetExit?.Invoke(gameObjectID, callerName);
    }

    public abstract float GetSize();
    public abstract void UpdateSize(float newSize);

    private ContactPoint2D[] cp;

    public abstract bool IsPositionInRange(Vector2 pos);

    

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
