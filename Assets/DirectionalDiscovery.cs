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
    
    [ShowInInspector]
    public (Vector2,DirectionalDiscovery,string)[] DirectionalDiscoveriesFound = new (Vector2,DirectionalDiscovery,string)[4]
    {
        (Vector2.zero,null,string.Empty),(Vector2.zero,null,string.Empty),(Vector2.zero,null,string.Empty),(Vector2.zero,null,string.Empty)
    };
    
    public RectTransform MyRectTransform {get => transform as RectTransform;}

    public Color[] DirectionalColors = new Color[4]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow
    };


    protected void Awake()
    {
        DirectionalDiscoveriesContainer.AllDirectionalDiscoveries.Add(_MyGameObjectID,(transform.position,this));
    }

    [Required]
    public MyGameObject ParentMyGameObject;

    public int _MyGameObjectID => ParentMyGameObject != null ? ParentMyGameObject.MyGameObjectID : MyGameObjectID;

    private void Start()
    {
        /*DirectionalDiscoveriesContainer.AllDirectionalDiscoveries.Add(_MyGameObjectID,(transform.position,this));*/
        name = ParentMyGameObject != null ? ParentMyGameObject.MyGameObjectID + "_" + name : name;
    }

    [Required]
    public DirectionalDiscoveriesManager DirectionalDiscoveriesContainer;
    
    [Button]
    public void GetDirectionalDiscoveriesOnObject()
    {
        GetDirectionalDiscoveries(DirectionalDiscoveriesContainer.AllDirectionalDiscoveries);
        for (int i = 0; i < DirectionalDiscoveriesFound.Length; i++)
        {
            if (DirectionalDiscoveriesFound[i].Item2 != null)
            {
                Debug.DrawLine(transform.position,DirectionalDiscoveriesFound[i].Item1,DirectionalColors[i],15f);
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
            var directionalDiscoveryNodes = DirectionalColliderGroupsByPriority[i].GetNodes();
            for (int j = 0; j < directionalDiscoveryNodes.Length; j++)
            {
                if (FoundFlags[j])
                {
                    continue;
                }

                string foundAreaName = directionalDiscoveryNodes[j].name;
                float foundDistance = 999f;
                int foundDirectionalColliderID = 0;
                foreach (var VARIABLE in dict)
                {
                    if (VARIABLE.Key == _MyGameObjectID)
                    {
                        continue;
                    }

                    bool check = false;
                    float _currentDistance = 999f;
                    
                    Vector3[] PHArray = new Vector3[4];
                    VARIABLE.Value.Item2.MyRectTransform.GetWorldCorners(PHArray);
                    for (int k = 0; k < PHArray.Length; k++)
                    {
                        check = directionalDiscoveryNodes[j].MyBoxCollider2D.Contains2d(PHArray[k], true);
                        _currentDistance = Vector2.Distance(transform.position, PHArray[k]);
                        if (check) { break;}
                    }
                    
                    if (check) {
                    
                    if (_currentDistance < foundDistance)
                        {
                            foundDirectionalColliderID = VARIABLE.Key;
                            foundDistance = _currentDistance;
                        }
                    }
                }

                if (foundDirectionalColliderID != 0)
                {
                    DirectionalDiscoveriesFound[j] = (dict[foundDirectionalColliderID].Item1,dict[foundDirectionalColliderID].Item2,foundAreaName);
                    FoundFlags[j] = true;
                    if (directionalDiscoveryNodes[j]._DEBUG)
                    {
                        Debug.DrawLine(dict[foundDirectionalColliderID].Item1, directionalDiscoveryNodes[j].transform.position,
                            Color.magenta, 20f);
                    }
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
    public DirectionalDiscoveryNode Up;
    [Required]
    [FoldoutGroup("Clockwise:")]
    public DirectionalDiscoveryNode Right;
    [Required]
    [FoldoutGroup("Clockwise:")]
    public DirectionalDiscoveryNode Down;
    [Required]
    [FoldoutGroup("Clockwise:")]
    public DirectionalDiscoveryNode Left;
    

    public DirectionalDiscoveryNode[] GetNodes()
    {
        DirectionalDiscoveryNode[] newBoxCollider2Ds = new DirectionalDiscoveryNode[4];
        newBoxCollider2Ds[0] = Up != null ? Up : null;
        newBoxCollider2Ds[1] = Right != null ? Right : null;
        newBoxCollider2Ds[2] = Down != null ? Down : null;
        newBoxCollider2Ds[3] = Left != null ? Left : null;
        return newBoxCollider2Ds;
    }
}


