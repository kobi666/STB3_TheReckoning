using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyBox;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using DegreeUtils;
using UniRx;
using UnitSpawning;

[Serializable]
public class SpawnSingleUnitToBasePosition : SpawnerBehavior
{
    public List<UnitPoolCreationData> unitCreationData = new List<UnitPoolCreationData>();
    
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
        GenericUnitController guc = ParentComponent.SpawnUnitInactive(UnitPools[0]);
        guc.transform.position = ParentComponent.transform.position;
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

    public MainPathController PathController;

    public Dictionary<SplineTypes,SplinePathController> PathSplines
    {
        get => PathController.SplinePaths;
    }

    private bool validatePathPoints()
    {
        return !PathPointTypesByPriority.IsNullOrEmpty();
    }

    [SerializeField]
    public List<UnitWave> Waves = new List<UnitWave>();
    
    [ShowInInspector]
    public Queue<bool[]> SpawnFormations = new Queue<bool[]>();
    public bool[] GetSpawnFormation()
    {
        bool[] formation = SpawnFormations.Dequeue();
        if (SpawnFormations.IsNullOrEmpty())
        {
            AllWavesFinished = true;
        }

        return formation;
    }



    public override void SpecificBehaviorInit()
    {
        int UnitCounter = 0;
        if (PathController == null) {
        PathController = PathPointFinder.PathSplines[PathPointFinder.FindShortestSpline()].parentPath;
        }
        onBehaviorEnd += delegate { SpawningWaveInProgress = false; };
        foreach (var wave in Waves)
        {
            wave.WaveInit();
            for (int i = 0; i <= wave.WaveDelayInSpawnTimeMultiplier; i++)
            {
                SpawnFormations.Enqueue(new []{false,false,false,false,false});
            }
            for (int i = 0; i <= wave.AmountOfBatches; i++)
            {
                for (int j = 0; j < wave.BatchStructure.FormationLength; j++)
                {
                    SpawnFormations.Enqueue(wave.BatchStructure.GetColumn(j));
                }
            }

            foreach (var spawnFormation in SpawnFormations)
            {
                foreach (var unit in spawnFormation)
                {
                    if (unit)
                    {
                        UnitCounter++;
                    }
                }
            }
        }

        GameManager.Instance.CurrentLevelManager.TotalUnitsInLevel += UnitCounter;

    }
    
    
    

    public Vector2? UnitBasePosition;

    public override bool BehaviorConditions()
    {
        return ParentComponent.NumberOfManagedUnits < ParentComponent.MaxUnits;
    }

    public override void Behavior()
    {
        if (!AllWavesFinished)
        {
            var _formation = GetSpawnFormation();
            for (int i = 0; i < _formation.Length; i++)
            {
                if (_formation[i])
                {
                    SpawnUnitByColumn(i);
                }
            }
        }
    }


    public bool SpawningWaveInProgress;
    public int formationRowIndex = 0;
    private float batchTimerCounter = 0;
    private float WaveTimerCounter = 0;
    private int waveCounter;
    private bool allWavesFinished = false;

    private bool AllWavesFinished
    {
        get => allWavesFinished;
        set
        {
            allWavesFinished = value;
            if (allWavesFinished)
            {
                BehaviorInProgress = false;
            }
        }
    }

    
    public void SpawnUnitByColumn(int splineIndex)
    {
        GenericUnitController guc = UnitPools[0].GetInactive();
        guc.PathWalker.SplinePathController = PathSplines[(SplineTypes)splineIndex];
        guc.PathWalker.Spline = guc.PathWalker.SplinePathController.BgCcMath;
        guc.Data.DynamicData.Spline = PathSplines[(SplineTypes)splineIndex].BgCcMath;
        //change to spawning position
        guc.transform.position = ParentComponent.transform.position;
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


