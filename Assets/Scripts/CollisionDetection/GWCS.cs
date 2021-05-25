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


    public bool DebugRegister;
    public void AddObject(CollidingObject _collidingObject, int collisionID)
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
    private struct JOB_ClearNewCollisions : IJob
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
            gridCells.Add(GetPositionHashMapKey(allSimulatedPositons[index].position, quadrentCellSize), allSimulatedPositons[index].CollisionID);
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
        public NativeMultiHashMap<int, int>.ParallelWriter newTotalCollisionsByCollisionID;

        

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
        
        private bool CheckOverLapBetweenTwoColliders(int collisionID, int otherCollisionID)
        {
            BittableSimulatedCollider collider = allSimulatedColliders[collisionID];
            BittableSimulatedCollider otherCollider = allSimulatedColliders[otherCollisionID];
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
        
        
        
        
        // at this point, index should be the same as collisionID
        public void Execute(int index)
        {
            FixedList64<int2> getCellKeys = GetCellKeysforsimulatedCollider(allSimulatedColliders[index]);
            var currentCollider = allSimulatedColliders[index];
            if (allSimulatedColliders[index].TypesICanDetect > 0)
            {
                detectorCheckedFlags.Set(allSimulatedColliders[index].CollisionID,true);
            }
            
            for (int i = 0; i < getCellKeys.Length; i++)
            {
                if (gridCells.TryGetFirstValue(getCellKeys[i], out int otherColliderIndex, out var iterator))
                {
                    do
                    {
                        if (!detectorCheckedFlags.IsSet(allSimulatedColliders[otherColliderIndex].CollisionID))
                        {
                            if (currentCollider.GameObjectID == allSimulatedColliders[otherColliderIndex].GameObjectID)
                            {
                                continue;
                            }
                            if (TestDetection(currentCollider.TypesICanDetect,
                                allSimulatedColliders[otherColliderIndex].DetectableType))
                            {
                                if (CheckOverLapBetweenTwoColliders(allSimulatedColliders[index].CollisionID, allSimulatedColliders[otherColliderIndex].CollisionID))
                                {
                                    if (allSimulatedColliders[otherColliderIndex].CollisionID != 0) {
                                    newTotalCollisionsByCollisionID.Add(allSimulatedColliders[index].CollisionID,allSimulatedColliders[otherColliderIndex].CollisionID);
                                    //Debug.LogWarning("added " + allSimulatedColliders[otherColliderIndex].CollisionID);
                                    }
                                }
                            }
                        }
                    } while (gridCells.TryGetNextValue(out otherColliderIndex, ref iterator));
                }
            }
            
        }
    }
    
    [BurstCompile]
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
        public NativeMultiHashMap<int, int>.ParallelWriter PersistnetCurrentCollisions;
        [Unity.Collections.ReadOnly]
        public NativeMultiHashMap<int, int> newTotalCollisionsByIndex;

        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;

        public void Execute(int index)
        {
            var newCollision = newTotalCollisionsByIndex.GetValuesForKey(allSimulatedColliders[index].CollisionID);
            /*while (newCollision.MoveNext()) {
                PersistnetCurrentCollisions.Add(allSimulatedColliders[index].CollisionID, newCollision.Current);*/
            while (newCollision.MoveNext())
            {
                if (newCollision.Current == 0)
                {
                    continue;
                }
                PersistnetCurrentCollisions.Add(allSimulatedColliders[index].CollisionID, newCollision.Current);
            }
        }
    }
    
    [BurstCompile]
    private struct JOB_resolveNewEnters : IJobParallelFor
    {
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> newTotalCollisionsByIndex;
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> OldCollisions;
        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;

        [NativeDisableParallelForRestriction] public NativeMultiHashMap<int, int>.ParallelWriter onEnterCollisions;


        public void Execute(int index)
        {
            if (index == 0)
            {
                return;
            }
            var newCollisionsPerValue = newTotalCollisionsByIndex.GetValuesForKey(allSimulatedColliders[index].CollisionID);
            bool noOldCollisions = OldCollisions.CountValuesForKey(index) <= 0;

            /*var oldColiisionsForDebug = OldCollisions.GetValuesForKey(allSimulatedColliders[index].CollisionID);*/
            /*while (oldColiisionsForDebug.MoveNext())
            {
                Debug.LogWarning("index number : " + index + " : " + oldColiisionsForDebug.Current);
            }

            while (newCollisionsPerValue.MoveNext())
            {
                Debug.LogWarning("index number : " + index + " : " + newCollisionsPerValue.Current);
            }
            newCollisionsPerValue.Reset();*/
            
            
            //NativeArray<int> collisionsToAdd = new NativeArray<int>(newTotalCollisionsByIndex.CountValuesForKey(index),Allocator.TempJob);

            if (noOldCollisions)
            {
                do
                {
                    if (newCollisionsPerValue.Current == 0)
                    {
                        continue;
                    }
                    Debug.LogWarning("Totally new collision found :" + newCollisionsPerValue.Current + " on detector ID " + allSimulatedColliders[index].CollisionID );
                    onEnterCollisions.Add(allSimulatedColliders[index].CollisionID, newCollisionsPerValue.Current);
                } while (newCollisionsPerValue.MoveNext());
            }

            if (!noOldCollisions)
            {
                var oldCollisionsPerValue = OldCollisions.GetValuesForKey(allSimulatedColliders[index].CollisionID);
                do
                {
                    bool NewCollisionFoundInOld = false;
                    if (newCollisionsPerValue.Current == 0)
                    {
                        continue;
                    }
                    
                    do
                    {
                        if (oldCollisionsPerValue.Current == 0)
                        {
                            continue;
                        }
                        if (newCollisionsPerValue.Current == oldCollisionsPerValue.Current)
                        {
                            Debug.LogWarning("Same Collision Found in Old and New");
                            NewCollisionFoundInOld = true;
                            break;
                        }
                    } while (oldCollisionsPerValue.MoveNext());
                    
                    
                    oldCollisionsPerValue.Reset();
                    if (!NewCollisionFoundInOld) {
                        while (oldCollisionsPerValue.MoveNext())
                        {
                            Debug.LogWarning("index number : " + index + " : " + oldCollisionsPerValue.Current);
                        }
                        Debug.LogWarning("Totally new collision found :" + newCollisionsPerValue.Current + " on detector ID " + allSimulatedColliders[index].CollisionID );
                        onEnterCollisions.Add(allSimulatedColliders[index].CollisionID, newCollisionsPerValue.Current);
                    }

                } while (newCollisionsPerValue.MoveNext());

            }
            //collisionsToAdd.Dispose();


            /*if (oldCollisions.Current == 0)
            {
                Debug.LogWarning("Current old collisions's current is 0");
                if (newCollisions.Current != 0) {
                    Debug.LogWarning("Current NEW collisions's current is NOT 0");
                    while (newCollisions.MoveNext())
                    {
                        Debug.LogWarning("Totally new collision found :" + newCollisions.Current + " on detector ID " +
                                         allSimulatedColliders[index].CollisionID);
                        onEnterCollisions.Add(allSimulatedColliders[index].CollisionID, newCollisions.Current);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Current Old Collisions is NOT 0, it is " + oldCollisions.Current);
                if (newCollisions.Current != 0) {
                    while (newCollisions.MoveNext())
                    {
                        if (newCollisions.Current == 0)
                        {
                            continue;
                        }

                        Debug.LogWarning("asdasd");
                        int collisionThatWasNotFound = 0;
                        bool newCollisionNotFoundInOld = true;
                        int checkCounter = 0;
                        while (oldCollisions.MoveNext())
                        {
                            if (oldCollisions.Current == 0)
                            {
                                continue;
                            }

                            if (oldCollisions.Current == newCollisions.Current)
                            {
                                newCollisionNotFoundInOld = false;
                                checkCounter++;
                                break;
                            }
                        }

                        if (newCollisionNotFoundInOld)
                        {
                            Debug.LogWarning("Totally new collision found :" + newCollisions.Current + " on detector ID " +
                                             allSimulatedColliders[index].CollisionID);
                            onEnterCollisions.Add(allSimulatedColliders[index].CollisionID, newCollisions.Current);
                            break;
                        }
                        oldCollisions.Reset();
                    }
                }
            }*/
        }
    }
    
    [BurstCompile]
    private struct JOB_resolveNewExits : IJobParallelFor
    {
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> newTotalCollisionsByIndex;
        [Unity.Collections.ReadOnly] public NativeMultiHashMap<int, int> OldCollisions;
        [Unity.Collections.ReadOnly] public NativeArray<BittableSimulatedCollider> allSimulatedColliders;

        [NativeDisableParallelForRestriction] public NativeMultiHashMap<int, int>.ParallelWriter onExitCollisions;

        public void Execute(int index)
        {
            if (index == 0)
            {
                return;
            }

            var newCollisions = newTotalCollisionsByIndex.GetValuesForKey(allSimulatedColliders[index].CollisionID);
            var oldCollisions = OldCollisions.GetValuesForKey(allSimulatedColliders[index].CollisionID);

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

    private bool executionInProgress;

    private void ExecuteCollisionSystem()
    {
        if (!executionInProgress)
        {
            executionInProgress = true;
        var _JOB_ClearGridCells = new JOB_ClearGridCells
        {
            gridCells = GridCells
        };
        var JH_cleargridCells = _JOB_ClearGridCells.Schedule();
        

        //remove objects from queue
        if (RemovalQueue.Count > 0)
        {
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

        if (AdditionQueue.Count > 0)
        {
            do
            {
                var objecttobeadded = AdditionQueue.Dequeue();
                AllObjects.Add(objecttobeadded.CollisionID, objecttobeadded);
                if (objecttobeadded.CollisionTagsICanDetectInt > 0)
                {
                    AllDetectors.Add(objecttobeadded.CollisionID, objecttobeadded as CollisionDetector);
                }

            } while (AdditionQueue.Count > 0);
        }

        NativeArray<BittableSimulatedCollider> AllSimulatedColliders =
            new NativeArray<BittableSimulatedCollider>(AllObjects.Count + 1, Allocator.TempJob);
        NativeBitArray DetectorCheckdFlags = new NativeBitArray(AllSimulatedColliders.Length + 1, Allocator.TempJob);
        NativeMultiHashMap<int, int> NewTotalCollisionsByIndex =
            new NativeMultiHashMap<int, int>(200000, Allocator.TempJob);

        int objCounter = 1;
        foreach (var _obj in AllObjects.Values)
        {
            var bc2d = _obj.BoxCollider2D;
            var size = bc2d.size;
            AllSimulatedColliders[_obj.CollisionID] = new BittableSimulatedCollider(size.x, size.y,
                (Vector2) _obj.transform.position + bc2d.offset,
                _obj.CollisionID,
                _obj.GameObjectID,
                _obj.CollisionTagInt,
                _obj.CollisionTagsICanDetectInt);
            objCounter++;
            //Debug.LogWarning(AllSimulatedColliders[_obj.GameObjectID].GameObjectID + " : " + AllSimulatedColliders[_obj.GameObjectID].CollisionID );
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
            newTotalCollisionsByCollisionID = NewTotalCollisionsByIndex.AsParallelWriter(),
        };
        
        
        //remove because there was no need to clear the new collisions since it's disposed in the end.
        /*var _JOB_ClearNewCollisions = new JOB_ClearNewCollisions
        {
            newTotalCollisions = NewTotalCollisionsByIndex
        };*/

        NativeMultiHashMap<int, int> OnEnterCollisions = new NativeMultiHashMap<int, int>(10000, Allocator.TempJob);
        NativeMultiHashMap<int, int> OnExitCollisions = new NativeMultiHashMap<int, int>(10000, Allocator.TempJob);

        var _JOB_GetNewEnterCollisions = new JOB_resolveNewEnters
        {
            allSimulatedColliders = AllSimulatedColliders,
            OldCollisions = CurrentCollisions,
            newTotalCollisionsByIndex = NewTotalCollisionsByIndex,
            onEnterCollisions = OnEnterCollisions.AsParallelWriter(),
        };

        var _JOB_GetNewExitCollisions = new JOB_resolveNewExits
        {
            allSimulatedColliders = AllSimulatedColliders,
            OldCollisions = CurrentCollisions,
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
            PersistnetCurrentCollisions = CurrentCollisions.AsParallelWriter(),
            newTotalCollisionsByIndex = NewTotalCollisionsByIndex
        };

//        Debug.LogWarning(CurrentCollisions.Count());
        JobHandle JH_fillGridCellsMap =
            _JOB_fillMapJob.Schedule(AllSimulatedColliders.Length, 128, JH_cleargridCells);

        JobHandle JH_GetcurrentCollisions =
            _JOB_GetCurrentCollisions.Schedule(AllSimulatedColliders.Length, 1, JH_fillGridCellsMap);





        JobHandle JH_GetNewEnterCollisions =
            _JOB_GetNewEnterCollisions.Schedule(AllSimulatedColliders.Length, 4, JH_GetcurrentCollisions);
        JobHandle JH_GetNewExitCollisions =
            _JOB_GetNewExitCollisions.Schedule(AllSimulatedColliders.Length, 4, JH_GetNewEnterCollisions);
        JobHandle JH_postResolveJobHandle = new JobHandle();
        JobHandle JH_clear_CURRENT_Colissions = _JOB_clearCurrentCollisions.Schedule(JH_GetNewExitCollisions);
        JobHandle JH_UpdateCurrentCollisions =
            _JOB_updateCurrentCollisions.Schedule(AllSimulatedColliders.Length, 128, JH_clear_CURRENT_Colissions);

        //JobHandle JH_clear_NEW_Collisions = _JOB_ClearNewCollisions.Schedule(JH_UpdateCurrentCollisions);


        JH_cleargridCells.Complete();
        JH_fillGridCellsMap.Complete();
        JH_GetcurrentCollisions.Complete();
        JH_GetNewEnterCollisions.Complete();
        JH_GetNewExitCollisions.Complete();
        OnEnterCollisions.GetEnumerator().Reset();
        OnExitCollisions.GetEnumerator().Reset();
        JH_clear_CURRENT_Colissions.Complete();
        JH_UpdateCurrentCollisions.Complete();
        /*if (CurrentCollisions.Count() > 0)
        {
            Debug.LogWarning("OLD Collision Count At end : " + CurrentCollisions.Count());
        }
        
        if (NewTotalCollisionsByIndex.Count() > 0)
        {
            Debug.LogWarning("NEW Collision Count At end : " + NewTotalCollisionsByIndex.Count());
        }*/
        foreach (var detector in AllDetectors)
        {

            var allNewEnters = OnEnterCollisions.GetValuesForKey(detector.Key);
            var allNewExits = OnExitCollisions.GetValuesForKey(detector.Key);
            int enterCounters = 0;
            while (allNewEnters.MoveNext())
            {
                enterCounters++;
                detector.Value.OnTargetEnter(allNewEnters.Current);
                //Debug.LogWarning("ENTER : " + detector.Key + " " + allNewEnters.Current);
            }
            
            //Debug.LogWarning("enter counter : " + enterCounters);

            int exitCounters = 0;
            while (allNewExits.MoveNext())
            {
                exitCounters++;
                detector.Value.OnTargetExit(allNewExits.Current, name);
                //Debug.LogWarning("EXIT : " + detector.Key + " " + allNewExits.Current);
            }
            

            //Debug.LogWarning("exit counter : " + exitCounters);
        }

        JH_postResolveJobHandle.Complete();

        //JH_clear_NEW_Collisions.Complete();





        // the part where I dispose of all the things
        AllSimulatedColliders.Dispose();
        DetectorCheckdFlags.Dispose();
        NewTotalCollisionsByIndex.Dispose();
        OnExitCollisions.Dispose();
        OnEnterCollisions.Dispose();
        int cc3 = 0;
        foreach (var VARIABLE in CurrentCollisions)
        {
            cc3++;
        }

        //Debug.LogWarning("Current Collisions : " + cc3);
        executionInProgress = false;
        }
}

    public bool EnableSystem = true;
    public int frameCounter = 0;
    public int UpdateRate = 10;
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
        AdditionQueue.Clear();
        RemovalQueue.Clear();
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
