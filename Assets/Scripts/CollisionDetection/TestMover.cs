using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Mathematics;

public class TestMover : MonoBehaviour
{
    private Vector2[] directions = new Vector2[]
    {
        Vector2.down, 
        Vector2.left, 
        Vector2.right, 
        Vector2.up,
        Vector2.up + Vector2.left,
        Vector2.up + Vector2.right,
        Vector2.down + Vector2.left,
        Vector2.down + Vector2.right
    };
    
    public Dictionary<int, TestMover> Targets = new Dictionary<int, TestMover>();

    public int DetectionID;

    [SerializeField] 
    public DetectionTypes DetectionType;
    [SerializeField]
    public List<DetectionTypes> DetectableTypes = new List<DetectionTypes>();

    public bool IsDetector;
    public bool IsDetectable = true;
    
    public BoxCollider2D BoxCollider2D;
    
    public int DetectableTypeInt;
    public int TypesICanDetectInt;

    int ConvertDetetableTypesToInt(List<DetectionTypes> detectionTypes)
    {
        int convertedInt = 0;
        
        foreach (var detectionType in detectionTypes)
        {
            if (detectionType == DetectionTypes.None)
            {
                if (detectionTypes.Count > 1)
                {
                    throw new Exception("None detected with other Detectable Types");
                }
                return 0;
            }
            convertedInt |= 1 << (int) detectionType;
        }

        
        return convertedInt;
    }

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        DetectionID = IDGenerator.Instance.GetCollisionID();
        if (DetectionType == DetectionTypes.None)
        {
            DetectableTypeInt = 0;
        }
        else
        {
            DetectableTypeInt = 1<<(int) DetectionType;    
        }
        
        TypesICanDetectInt = ConvertDetetableTypesToInt(DetectableTypes);

        //onTargetEntered += debugID;
        onTargetEntered += addToTargets;
        //onTargetExit += debugID;
        onTargetExit += removeFromTargets;
    }

    void addToTargets(int i)
    {
        if (!CurrentTargetsID.Contains(i))
        {
            CurrentTargetsID.Add(i);
        }
    }
    
    void removeFromTargets(int i)
    {
        if (CurrentTargetsID.Contains(i))
        {
            CurrentTargetsID.Remove(i);
        }
    }

    void debugID(int id)
    {
        Debug.Log(id);
    }

    private float directioncounter;
    public Vector2 DirectionV2;
    
    [ShowInInspector]
    public ConcurrentDictionary<int,int> AllTargetsIveEncountered = new ConcurrentDictionary<int,int>();
    
    public List<int> CurrentTargetsID = new List<int>();
    private void Start()
    {
        DirectionV2 = directions[Random.Range(0, directions.Length)];
        UnitMover.Instance.OnMove += MoveInRandomDirection;
        BoxCollider2D.enabled = false;
        /*TheGreatWorkingCollisionSystem.AllTestMovers.TryAdd(DetectionID,this);
        if (DetectableTypes.Count > 0)
        {
            TheGreatWorkingCollisionSystem.AllDetectors.TryAdd(DetectionID, this);
        }*/
        
        //GWCS.AddObject(this);
    }

    public event Action<int> onTargetEntered;

    public void OnTargetEnter(int objectID)
    {
        onTargetEntered?.Invoke(objectID);
    }
    public event Action<int> onTargetExit;

    public void OnTargetExit(int objectID)
    {
        onTargetExit?.Invoke(objectID);
    }

    void MoveInRandomDirection()
    {
        directioncounter += Time.deltaTime;
        if (directioncounter > 7f)
        {
            DirectionV2 = directions[Random.Range(0, directions.Length)];
            directioncounter = 0;
        }

        var transform1 = transform;
        var position = transform1.position;
        Vector2 targetPos = position + (Vector3)DirectionV2;
        Vector2 t_targetPos = new Vector2(Mathf.Clamp(targetPos.x, -15,15), Mathf.Clamp(targetPos.y,-15,15));
        transform.position = Vector2.MoveTowards(position, t_targetPos, Time.deltaTime * Speed);
    }

    public float Speed = 0.5f;


    // Update is called once per frame

}


//add offset and rotation
public struct BittableSimulatedCollider 
{
    public float2 position;
    public float width;
    public float height;
    public int CollisionID;
    public int GameObjectID;
    public int DetectableType;
    public int TypesICanDetect;
    public bool DebugCollider;

    public BittableSimulatedCollider(float _width, float _height, Vector2 _position, int collisionID, int gameObjectID, int detectableType, int typesICanDetect, bool debugCollider)
    {
        position = _position;
        width = _width;
        height = _height;
        CollisionID = collisionID;
        DetectableType = detectableType;
        TypesICanDetect = typesICanDetect;
        GameObjectID = gameObjectID;
        DebugCollider = debugCollider;
    }
}









