using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class DirectionalDiscovery : MyGameObject
{
    public DirectionalColliders[] DirectionalColliderGroupsByPriority = new DirectionalColliders[5];

    public DirectionalDiscovery[] DirectionalDiscoveriesFound = new DirectionalDiscovery[4]
    {
        null, null, null, null
    };

    private void Start()
    {
        DirectionalDiscoveriesContainer.AllDirectionalDiscoveries.Add(MyGameObjectID,(transform.position,this));
    }

    [Required]
    public DirectionalDiscoveriesManager DirectionalDiscoveriesContainer;
    
    [Button]
    public void GetDirectionalDiscoveriesOnObject()
    {
        Color randomColor = new Color(Random.Range(0f,255f),Random.Range(0f,255f),Random.Range(0f,255f),255f);
        GetDirectionalDiscoveries(DirectionalDiscoveriesContainer.AllDirectionalDiscoveries);
        foreach (var dd  in DirectionalDiscoveriesFound)
        {
            if (dd != null)
            {
                Debug.DrawLine(transform.position,dd.transform.position, randomColor, 20f);
            }
        }
    }

    public void GetDirectionalDiscoveries(Dictionary<int, (Vector2, DirectionalDiscovery)> dict)
    {
        
        bool[] FoundFlags = new bool[4]
        {
            false, false, false, false
        };
        for (int i = 0; i < DirectionalColliderGroupsByPriority.Length; i++)
        {
            var directionalColliders = DirectionalColliderGroupsByPriority[i].GetCollider2D();
            for (int j = 0; j < directionalColliders.Length; j++)
            {
                if (FoundFlags[j])
                {
                    continue;
                }
                float foundDistance = 999f;
                int foundDirectionalColliderID = 0;
                foreach (var VARIABLE in dict)
                {
                    if (VARIABLE.Key == MyGameObjectID)
                    {
                        continue;
                    }
                    
                    if (directionalColliders[j].Contains2d(VARIABLE.Value.Item1)) {
                    var _currentDistance = Vector2.Distance(transform.position, VARIABLE.Value.Item1);
                    if (_currentDistance < foundDistance)
                        {
                            foundDirectionalColliderID = VARIABLE.Key;
                            foundDistance = _currentDistance;
                        }
                    }
                }

                if (foundDirectionalColliderID != 0)
                {
                    DirectionalDiscoveriesFound[j] = dict[foundDirectionalColliderID].Item2;
                    FoundFlags[j] = true;
                }
            }
        }
    }
    
}

[System.Serializable]
public class DirectionalColliders
{
    
    [Required]
    [FoldoutGroup("Clockwise:")]
    public BoxCollider2D Up;
    [Required]
    [FoldoutGroup("Clockwise:")]
    public BoxCollider2D Right;
    [Required]
    [FoldoutGroup("Clockwise:")]
    public BoxCollider2D Down;
    [Required]
    [FoldoutGroup("Clockwise:")]
    public BoxCollider2D Left;
    

    public BoxCollider2D[] GetCollider2D()
    {
        BoxCollider2D[] newBoxCollider2Ds = new BoxCollider2D[4];
        newBoxCollider2Ds[0] = Up != null ? Up : null;
        newBoxCollider2Ds[1] = Right != null ? Right : null;
        newBoxCollider2Ds[2] = Down != null ? Down : null;
        newBoxCollider2Ds[3] = Left != null ? Left : null;
        return newBoxCollider2Ds;
    }
}


