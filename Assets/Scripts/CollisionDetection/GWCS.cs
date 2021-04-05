using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Unity;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

[DefaultExecutionOrder(-20)]
public class GWCS : MonoBehaviour
{
    public static GWCS instance;
    
    [ShowInInspector] private Dictionary<int, CollidingObject> AllObjects = new Dictionary<int, CollidingObject>();

    [ShowInInspector] private Dictionary<int, CollisionDetector> AllDetectors = new Dictionary<int, CollisionDetector>();

    [ShowInInspector] private Queue<CollidingObject> AdditionQueue = new Queue<CollidingObject>();

    [ShowInInspector] private Queue<CollidingObject> RemovalQueue = new Queue<CollidingObject>();

    public void AddObject(CollidingObject _collidingObject)
    {
        AdditionQueue.Enqueue(_collidingObject);
    }

    public void RemoveObject(CollidingObject _collidingObject)
    {
        RemovalQueue.Enqueue(_collidingObject);
    }


    public NativeMultiHashMap<int2, int> GridCells;

    public NativeMultiHashMap<int, int> CurrentCollisions;

    //equal to maximum range
    public float QuadrentCellSize = 5;

    private void Awake()
    {
        instance = this;
        GridCells = new NativeMultiHashMap<int2, int>(10000, Allocator.Persistent);
        CurrentCollisions = new NativeMultiHashMap<int, int>(200000, Allocator.Persistent);
    }
    
    [BurstCompile]
    private struct JOB_ClearGridCells : IJob
    {
        public NativeMultiHashMap<int2, int> gridCells;

        public void Execute()
        {
            gridCells.Clear();
        }
    }
    
    [BurstCompile]
    private struct JOB_ClerNewCollisions : IJob
    {
        public NativeMultiHashMap<int, int> newTotalCollisions;
        public void Execute()
        {
           newTotalCollisions.Clear(); 
        }
    }

    [BurstCompile]
    private struct JOB_FillGridCellMap : IJobParallelFor
    {
        private int2 GetPositionHashMapKey(float2 position, float quadrantCellSize)
        {
            int2 _int2 = new int2((int) math.floor(position.x / quadrantCellSize),
                (int) (math.floor(position.y / quadrantCellSize)));
            return _int2;
        }

        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedPositons;

        public float quadrentCellSize;


        public NativeMultiHashMap<int2, int>.ParallelWriter gridCells;

        public void Execute(int index)
        {
            gridCells.Add(GetPositionHashMapKey(allSimulatedPositons[index].position, quadrentCellSize), index);
        }
    }
    
    
    [BurstCompile]
    private struct JOB_GetCurrentCollisions : IJobParallelFor
    {
        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int2, int> gridCells;
        [NativeDisableParallelForRestriction] 
        public NativeBitArray detectorCheckedFlags;
        [NativeDisableParallelForRestriction]
        public NativeMultiHashMap<int, int>.ParallelWriter newTotalCollisionsByIndex;

        

        public float quadrentCellSize;
        
        private int2 GetPositionHashMapKey(float2 position, float quadrantCellSize)
        {
            int2 _int2 = new int2((int)math.floor(position.x / quadrantCellSize),(int)(math.floor(position.y / quadrantCellSize)));
            return _int2;
        }
        
        private FixedList64<int2> GetCellKeysforsimulatedCollider(BittableSimulatedCollider bittableSimulatedCollider)
        {
            FixedList64<int2> allKeys = new FixedList64<int2>();
            float halfHeight = bittableSimulatedCollider.height / 2;
            float halfwidth = bittableSimulatedCollider.width / 2;
            
            int2 upRight = GetPositionHashMapKey(bittableSimulatedCollider.position + new float2(halfwidth, halfHeight),
                quadrentCellSize);
            int2 downRight = GetPositionHashMapKey(bittableSimulatedCollider.position + new float2(halfwidth, -halfHeight),
                quadrentCellSize);
            int2 downleft = GetPositionHashMapKey(bittableSimulatedCollider.position + new float2(-halfwidth, -halfHeight),
                quadrentCellSize);
            int2 upLeft = GetPositionHashMapKey(bittableSimulatedCollider.position + new float2(-halfwidth, halfHeight),
                quadrentCellSize);
            
            allKeys.Add(downleft);
            if (downleft.x != downRight.x)
            {
                allKeys.Add(downRight);
            }

            if (downleft.y != upLeft.y)
            {
                allKeys.Add(upLeft);
            }

            if (downRight.y != upLeft.y)
            {
                allKeys.Add(upRight);
            }

            return allKeys;
        }
        
        private bool CheckOverLapBetweenTwoColliders(int colliderIndex, int otherColliderIndex)
        {
            BittableSimulatedCollider collider = allSimulatedColliders[colliderIndex];
            BittableSimulatedCollider otherCollider = allSimulatedColliders[otherColliderIndex];
            return 
                (collider.position.x - collider.width / 2 <= otherCollider.position.x + otherCollider.width / 2 &&
                 collider.position.x + collider.width / 2 >= otherCollider.position.x - otherCollider.width / 2 &&
                 collider.position.y - collider.height / 2 <= otherCollider.position.y + otherCollider.height / 2 &&
                 collider.position.y + collider.height / 2 >= otherCollider.position.y - otherCollider.height / 2);
        }
        
        private bool TestDetection(int TypesICanDetect, int TargetDetectionType)
        {
            /*var a = Convert.ToString(TypesICanDetect, toBase: 8);
            var b = Convert.ToString(TargetDetectionType, toBase: 8);*/
            bool check = (TypesICanDetect & TargetDetectionType) != 0;
            return check;
        }
        
        
        
        
        
        public void Execute(int index)
        {
            FixedList64<int2> getCellKeys = GetCellKeysforsimulatedCollider(allSimulatedColliders[index]);
            var currentCollider = allSimulatedColliders[index];
            if (allSimulatedColliders[index].TypesICanDetect > 0)
            {
                detectorCheckedFlags.Set(index,true);
            }
            
            for (int i = 0; i < getCellKeys.Length; i++)
            {
                if (gridCells.TryGetFirstValue(getCellKeys[i], out int otherColliderIndex, out var iterator))
                {
                    do
                    {
                        if (!detectorCheckedFlags.IsSet(otherColliderIndex))
                        {
                            if (currentCollider.GameObjectID == allSimulatedColliders[otherColliderIndex].GameObjectID)
                            {
                                continue;
                            }
                            if (TestDetection(currentCollider.TypesICanDetect,
                                allSimulatedColliders[otherColliderIndex].DetectableType))
                            {
                                if (CheckOverLapBetweenTwoColliders(index, otherColliderIndex))
                                {
                                    newTotalCollisionsByIndex.Add(index,allSimulatedColliders[otherColliderIndex].CollisionID);
                                }
                            }
                        }
                    } while (gridCells.TryGetNextValue(out otherColliderIndex, ref iterator));
                }
            }
            
        }
    }
    
    
    private struct JOB_ClearCurrentCollisionsJob : IJob
    {

        public NativeMultiHashMap<int, int> currentCollisions;
        public void Execute()
        {
            currentCollisions.Clear();
        }
    }
    
    [BurstCompile]
    private struct JOB_UpdateCurrentCollisions : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeMultiHashMap<int, int>.ParallelWriter currentCollisions;
        [Unity.Collections.ReadOnly]
        public NativeMultiHashMap<int, int> newTotalCollisionsByIndex;

        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;

        public void Execute(int index)
        {
            var newCollision = newTotalCollisionsByIndex.GetValuesForKey(index);
            while (newCollision.MoveNext()) {
            currentCollisions.Add(allSimulatedColliders[index].CollisionID, newCollision.Current);
            }
        }
    }
    
    [BurstCompile]
    private struct JOB_resolveNewEnters : IJobParallelFor
    {
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> newTotalCollisionsByIndex;
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> currentCollisions;
        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;

        [NativeDisableParallelForRestriction] public NativeMultiHashMap<int, int>.ParallelWriter onEnterCollisions;


        public void Execute(int index)
        {
            if (index == 0)
            {
                return;
            }
            var newCollisions = newTotalCollisionsByIndex.GetValuesForKey(index);
            var oldCollisions = currentCollisions.GetValuesForKey(index);

            while (newCollisions.MoveNext())
            {
                bool newCollisionNotFoundInOld = true;
                while (oldCollisions.MoveNext())
                {
                    if (oldCollisions.Current == newCollisions.Current)
                    {
                        
                        newCollisionNotFoundInOld = false;
                        break;
                    }
                }
                oldCollisions.Reset();
                if (newCollisionNotFoundInOld)
                {
                    //Debug.Log("Totally new collision found :" + newCollisions.Current);
                    onEnterCollisions.Add(allSimulatedColliders[index].CollisionID, newCollisions.Current);
                }
            }
            
        }
    }
    
    [BurstCompile]
    private struct JOB_resolveNewExits : IJobParallelFor
    {
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> newTotalCollisionsByIndex;
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> currentCollisions;
        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;

        [NativeDisableParallelForRestriction] public NativeMultiHashMap<int, int>.ParallelWriter onExitCollisions;

        public void Execute(int index)
        {
            if (index == 0)
            {
                return;
            }

            var newCollisions = newTotalCollisionsByIndex.GetValuesForKey(index);
            var oldCollisions = currentCollisions.GetValuesForKey(index);

            while (oldCollisions.MoveNext())
            {
                if (oldCollisions.Current == 0)
                {
                    continue;
                }
                bool oldCollisionFound = false;
                while (newCollisions.MoveNext())
                {
                    
                    if (oldCollisions.Current == newCollisions.Current)
                    {
                        oldCollisionFound = true;
                        break;
                    }
                }
                newCollisions.Reset();
                if (!oldCollisionFound)
                {
                    /*Debug.Log("Found new Exit : " + oldCollisions.Current);*/
                    onExitCollisions.Add(allSimulatedColliders[index].CollisionID, oldCollisions.Current);
                }
            }
        }
    }

    private void ExecuteCollisionSystem()
    {
        var _JOB_ClearGridCells = new JOB_ClearGridCells
        {
            gridCells = GridCells
        };
        var JH_cleargridCells = _JOB_ClearGridCells.Schedule();

        
        //remove objects from queue
        if (RemovalQueue.Count > 0) {
        do
            {
                var objectToBeremoved = RemovalQueue.Dequeue();
                AllObjects.Remove(objectToBeremoved.CollisionID);
                if (objectToBeremoved.CollisionTagsICanDetectInt > 0)
                {
                    AllDetectors.Remove(objectToBeremoved.CollisionID);
                }
            } while (RemovalQueue.Count > 0);
        }
        
        //add objects from queue
        
        if (AdditionQueue.Count > 0) {
            do
            {
                var objecttobeadded = AdditionQueue.Dequeue();
                AllObjects.Add(objecttobeadded.CollisionID,objecttobeadded);
                if (objecttobeadded.CollisionTagsICanDetectInt > 0)
                {
                    AllDetectors.Add(objecttobeadded.CollisionID,objecttobeadded as CollisionDetector);
                }

            } while (AdditionQueue.Count > 0);
        }
        
        NativeArray<BittableSimulatedCollider> AllSimulatedColliders = new NativeArray<BittableSimulatedCollider>(AllObjects.Count + 1, Allocator.TempJob);
        NativeBitArray DetectorCheckdFlags = new NativeBitArray(AllSimulatedColliders.Length + 1,Allocator.TempJob);
        NativeMultiHashMap<int,int> NewTotalCollisionsByIndex = new NativeMultiHashMap<int, int>(200000, Allocator.TempJob);
        
        int objCounter = 1;
        foreach (var _obj in AllObjects.Values)
        {
            var size = _obj.BoxCollider2D.size;
            AllSimulatedColliders[objCounter] = new BittableSimulatedCollider(size.x, size.y,
                _obj.transform.position,
                _obj.CollisionID,
                _obj.GameObjectID,
                _obj.CollisionTagInt,
                _obj.CollisionTagsICanDetectInt);
            objCounter++;
        }

        var _JOB_fillMapJob = new JOB_FillGridCellMap
        {
            allSimulatedPositons = AllSimulatedColliders,
            quadrentCellSize = QuadrentCellSize,
            gridCells = GridCells.AsParallelWriter()
        };

        var _JOB_GetCurrentCollisions = new JOB_GetCurrentCollisions
        {
            allSimulatedColliders = AllSimulatedColliders,
            detectorCheckedFlags = DetectorCheckdFlags,
            gridCells = GridCells,
            quadrentCellSize = QuadrentCellSize,
            newTotalCollisionsByIndex = NewTotalCollisionsByIndex.AsParallelWriter(),
        };

        var _JOB_ClearNewCollisions = new JOB_ClerNewCollisions
        {
            newTotalCollisions = NewTotalCollisionsByIndex
        };
        
        NativeMultiHashMap<int,int> OnEnterCollisions = new NativeMultiHashMap<int, int>(10000, Allocator.TempJob);
        NativeMultiHashMap<int,int> OnExitCollisions = new NativeMultiHashMap<int, int>(10000, Allocator.TempJob);

        var _JOB_GetNewEnterCollisions = new JOB_resolveNewEnters
        {
            allSimulatedColliders = AllSimulatedColliders,
            currentCollisions = CurrentCollisions,
            newTotalCollisionsByIndex = NewTotalCollisionsByIndex,
            onEnterCollisions = OnEnterCollisions.AsParallelWriter(),
        };

        var _JOB_GetNewExitCollisions = new JOB_resolveNewExits
        {
            allSimulatedColliders = AllSimulatedColliders,
            currentCollisions = CurrentCollisions,
            newTotalCollisionsByIndex = NewTotalCollisionsByIndex,
            onExitCollisions = OnExitCollisions.AsParallelWriter()
        };

        var _JOB_clearCurrentCollisions = new JOB_ClearCurrentCollisionsJob
        {
            currentCollisions = CurrentCollisions
        };

        var _JOB_updateCurrentCollisions = new JOB_UpdateCurrentCollisions
        {
            allSimulatedColliders = AllSimulatedColliders,
            currentCollisions = CurrentCollisions.AsParallelWriter(),
            newTotalCollisionsByIndex = NewTotalCollisionsByIndex
        };
        

        JobHandle JH_fillGridCellsMap =
            _JOB_fillMapJob.Schedule(AllSimulatedColliders.Length, 128, JH_cleargridCells);
        
        JobHandle JH_GetcurrentCollisions = _JOB_GetCurrentCollisions.Schedule(AllSimulatedColliders.Length, 1, JH_fillGridCellsMap);

        
        
        
        
        JobHandle JH_GetNewEnterCollisions =
            _JOB_GetNewEnterCollisions.Schedule(AllSimulatedColliders.Length, 4, JH_GetcurrentCollisions);
        JobHandle JH_GetNewExitCollisions =
            _JOB_GetNewExitCollisions.Schedule(AllSimulatedColliders.Length, 4, JH_GetNewEnterCollisions);
        JobHandle JH_postResolveJobHandle = new JobHandle();
        JobHandle JH_clear_CURRENT_Colissions = _JOB_clearCurrentCollisions.Schedule(JH_GetNewExitCollisions);
        JobHandle JH_UpdateCurrentCollisions =
            _JOB_updateCurrentCollisions.Schedule(AllSimulatedColliders.Length, 128, JH_clear_CURRENT_Colissions);
        
        JobHandle JH_clear_NEW_Collisions = _JOB_ClearNewCollisions.Schedule(JH_UpdateCurrentCollisions);
        
        
        JH_cleargridCells.Complete();
        JH_fillGridCellsMap.Complete();
        JH_GetcurrentCollisions.Complete();
        JH_GetNewEnterCollisions.Complete();
        JH_GetNewExitCollisions.Complete();
        foreach (var detector in AllDetectors)
        {
            var allNewEnters = OnEnterCollisions.GetValuesForKey(detector.Key);
            var allNewExits = OnExitCollisions.GetValuesForKey(detector.Key);
            while (allNewEnters.MoveNext()) 
            {
                detector.Value.OnTargetEnter(allNewEnters.Current);
            }
                
            while (allNewExits.MoveNext())
            {
                detector.Value.OnTargetExit(allNewExits.Current,name);
            }
        }
        JH_postResolveJobHandle.Complete();
        
        JH_clear_NEW_Collisions.Complete();
        
        
        
        
        
        // the part where I dispose of all the things
        AllSimulatedColliders.Dispose();
        DetectorCheckdFlags.Dispose();
        NewTotalCollisionsByIndex.Dispose();
        OnExitCollisions.Dispose();
        OnEnterCollisions.Dispose();
    }

    public bool EnableSystem = true;
    public int frameCounter = 0;
    public int UpdateRate = 3;
    private void Update()
    {
        if (frameCounter > UpdateRate) {
        ExecuteCollisionSystem();
        frameCounter = 0;
        }
        frameCounter++;
    }

    private void OnDisable()
    {
        CurrentCollisions.Dispose();
        GridCells.Dispose();
    }
}

public enum DetectionTypes 
{
    Unit = 0,
    Weapon = 1,
    Projectile = 2,
    Path = 3,
    None = 99
}
