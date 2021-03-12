using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyBox;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using DegreeUtils;
using UnitSpawning;

[Serializable]
public class SpawnSingleUnitToBasePosition : SpawnerBehavior
{
    public List<UnitPoolCreationData> unitCreationData = new List<UnitPoolCreationData>();
    public override List<UnitPoolCreationData> UnitCreationData { get => unitCreationData; }
    private PoolObjectQueue<GenericUnitController> UnitQueuePool;
    public float distanceFromBasePosition = 0.5f;
    
    [ShowInInspector]
    private Vector2?[] UnitRallyPoints;
    private void GetUnitRallyPositions()
    {
        UnitRallyPoints = ParentComponent.GetSpreadPositions(ParentComponent.MaxUnits, UnitBasePosition, distanceFromBasePosition);
    }

    private Vector2? unitBasePosition;

    public Vector2? UnitBasePosition
    {
        get => unitBasePosition;
        set
        {
            unitBasePosition = value;
            if (unitBasePosition != null)
            {
                if (ParentComponent.Autonomous)
                {
                    OnBehaviorStart();
                }
            }
        }
    }


    [ValidateInput("validatePathPoints", "Pick at least one")]
    public PathPointFinder.PathPointType[] PathPointTypesByPriority;

    private bool validatePathPoints()
    {
        return !PathPointTypesByPriority.IsNullOrEmpty();
    }
    public bool SpawnMaxUnitsOnStartup;
    public override async void SpecificBehaviorInit()
    {
        UnitQueuePool = UnitCreationData[0].CreateUnitPool(ParentComponent.MaxUnits);
        onPositionRecalculation += SetUnitBasePosition;
        onPositionRecalculation += GetUnitRallyPositions;
        ParentComponent.onMaxUnitsChanged += GetUnitRallyPositions;
    }

    
    
    void SetUnitBasePosition()
    {
        if (SpecifiedBasePosition == null)
        {
            UnitBasePosition = PathPointFinder.GetPathPointByPriority(PathPointTypesByPriority);
        }
        else
        {
            UnitBasePosition = SpecifiedBasePosition;
        }
    }

    
    
    

    public override bool BehaviorConditions()
    {
        return ParentComponent.NumberOfManagedUnits < ParentComponent.MaxUnits;
    }

    public override void Behavior()
    {
        if (SpawnCounter >= SpawnInterval)
        {
            SpawnUnitToBasePosition();
            SpawnCounter = 0;
        }
        
    }

    private int spawningIndex;

    public int SpawningIndex
    {
        get
        {
            if (ParentComponent.MaxUnits == 1)
            {
                return 0;
            }
            else
            {
                return spawningIndex;
            }
        }
        set
        {
            if (value > ParentComponent.MaxUnits)
            {
                spawningIndex = 0;
            }
            else
            {
                spawningIndex = value;
            }
        }
    }

    void SpawnUnitToBasePosition()
    {
        GenericUnitController guc = ParentComponent.SpawnUnitInactive(UnitQueuePool);
        if (PathPointTypesByPriority[0] == PathPointFinder.PathPointType.RandomPosition)
        {
            guc.Data.DynamicData.BasePosition = new Vector2(Random.Range(-100f,100f),Random.Range(-100f,100f)); 
        }
        else {
        guc.Data.DynamicData.BasePosition = UnitRallyPoints[SpawningIndex];
        SpawningIndex++;
        }
        guc.gameObject.SetActive(true);
    }

    public override List<Effect> GetEffectList()
    {
        throw new NotImplementedException();
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new NotImplementedException();
    }

    public override void SetEffectList(List<Effect> effects)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class SpawnWaves : SpawnerBehavior
{

    public GenericUnitController TestEnemy;
    [ValidateInput("validatePathPoints", "Pick at least one")]
    public PathPointFinder.PathPointType[] PathPointTypesByPriority;

    public PathController PathController;

    public SortedList<SplineTypes,SplinePathController> PathSplines
    {
        get => PathController.ChildSplines;
    }

    private bool validatePathPoints()
    {
        return !PathPointTypesByPriority.IsNullOrEmpty();
    }
    
    
    public override List<UnitPoolCreationData> UnitCreationData { get; }
    
    [SerializeField]
    public List<UnitWave> Waves = new List<UnitWave>();
    
    
    

    public override void SpecificBehaviorInit()
    {
        if (PathController == null) {
        PathController = PathPointFinder.PathSplines[PathPointFinder.FindShortestSpline()].parentPath;
        }
        onBehaviorEnd += delegate { SpawningWaveInProgress = false; };
        
    }
    

    public Vector2? UnitBasePosition;

    public override bool BehaviorConditions()
    {
        return ParentComponent.NumberOfManagedUnits < ParentComponent.MaxUnits;
    }

    public bool SpawningWaveInProgress;
    public int formationRowIndex = 0;
    private float batchTimerCounter = 0;
    private float WaveTimerCounter = 0;
    private int waveCounter;
    public override async void Behavior()
    {
        if (SpawningWaveInProgress == true)
        {
            return;
        }

        if (SpawningWaveInProgress == false)
        {
            foreach (var wave in Waves)
            {
                while (!wave.WaveFinished)
                {
                    foreach (var batch in wave.BatchGroup)
                    {
                        while (!batch.AllBatchesFinished)
                        {
                            for (int aa = 0; aa < batch.AmountOfBatches; aa++)
                            {
                                for (int bb = 0; bb < batch.BatchStructure.GetLength(1) ; bb++)
                                {
                                    await Task.Delay(batch.TimeBetweenRowsMS);
                                    bool[] formation = batch.GetRow();
                                    for (int cc = 0; cc < formation.Length; cc++)
                                    {
                                        if (formation[cc])
                                        {
                                            SpawnUnitByColumn(cc);
                                        }
                                        await Task.Yield();
                                    }
                                }
                            }
                            
                            await Task.Yield();
                        }
                    }

                    await Task.Yield();
                }
            }

            SpawningWaveInProgress = false;
        }
    }

    private int namecounter;
    public void SpawnUnitByColumn(int splineIndex)
    {
        GenericUnitController guc = GameObject.Instantiate(TestEnemy);
        guc.PathWalker.SplinePathController = PathSplines[(SplineTypes)splineIndex];
        guc.PathWalker.Spline = guc.PathWalker.SplinePathController.BgCcMath;
        guc.Data.DynamicData.Spline = PathSplines[(SplineTypes)splineIndex].BgCcMath;
        //change to spawning position
        guc.transform.position = ParentComponent.transform.position;
        guc.name = guc.name + namecounter;
        namecounter++;
        guc.Data.DynamicData.BasePosition = PathSplines[(SplineTypes)splineIndex].splinePoints[0];
        guc.gameObject.SetActive(true);
    }

    public override List<Effect> GetEffectList()
    {
        return null;
    }

    public override void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
       
    }

    public override void SetEffectList(List<Effect> effects)
    {
       
    }
}


    
    
    public enum SpawningStates
    {
        Default,
        CantSpawn,
        Turbo
    }


