using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DistanceTester : MonoBehaviour
{
    public List<Transform> Targets = new List<Transform>();
    [ShowInInspector]
    public Dictionary<Vector2,(Transform,bool)> TargetsDict = new Dictionary<Vector2, (Transform,bool)>();

    public float radius;

    private void Awake()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }

    Vector2 calculateDistanceV2(Vector2 pos)
    {
        return (Vector2)transform.position - pos;
    }

    bool CheckIfV2InRadius(Vector2 pos)
    {
        Vector2 self = transform.position;
        if ((self - pos).sqrMagnitude < radius * radius)
        {
            return true;
        }

        return false;
    }

    public void calcTargetsVector2Ration()
    {
        foreach (var t in Targets)
        {
            Vector2 position = t.position;
            TargetsDict.Add(calculateDistanceV2(position),(t,CheckIfV2InRadius(position)));
        }
    }
    void Start()
    {
        calcTargetsVector2Ration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
