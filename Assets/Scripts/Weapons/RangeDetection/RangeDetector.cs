using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RangeDetector : MonoBehaviour
{

    [TagSelector]
     public string[] DiscoverableTags = new string[] { };
    // Start is called before the first frame update
    public event Action<GameObject> onTargetEnter;
    public void OnTargetEnter(GameObject target) {
        onTargetEnter?.Invoke(target);
    }

    public event Action<string> onTargetExit;
    public void OnTargetExit(string targetName) {
        onTargetExit?.Invoke(targetName);
    }


}
