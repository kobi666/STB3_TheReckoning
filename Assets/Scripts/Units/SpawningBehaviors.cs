using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;

///public classes used in this class are PathPointFinder : ParentComponent
/// 



    [Serializable]
    public class SpawnSingleUnitToBasePosition : SpawnerBehavior
    {
        public List<UnitPoolCreationData> unitCreationData = new List<UnitPoolCreationData>();
        public override List<UnitPoolCreationData> UnitCreationData { get => unitCreationData; }
        private PoolObjectQueue<GenericUnitController> UnitQueuePool;
        
        
        
        public Vector2? UnitBasePosition;
        public float SpawningCounter;
        public float SpawnInterval = 5f;
        
        [ValidateInput("validatePathPoints", "Pick at least one")]
        public PathPointFinder.PathPointType[] PathPointTypesByPriority;

        private bool validatePathPoints()
        {
            return !PathPointTypesByPriority.IsNullOrEmpty();
        }
        public bool SpawnMaxUnitsOnStartup;
        public override void SpecificBehaviorInit()
        {
            UnitQueuePool = UnitCreationData[0].CreateUnitPool();
            UnitBasePosition = PathPointFinder.GetPathPointByPriority(PathPointTypesByPriority);
        }

        
        
        

        public override bool BehaviorConditions()
        {
            return !ParentComponent.MaxUnitsReached;
        }

        public override void Behavior()
        {
            if (SpawningCounter < SpawnInterval)
            {
                SpawningCounter += StaticObjects.DeltaGameTime;
            }

            if (SpawningCounter >= SpawnInterval)
            {
                SpawnUnitToBasePosition();
                SpawningCounter = 0;
            }
            
        }

        private int spawningIndex;

        public int SpawningIndex
        {
            get => spawningIndex;
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
            guc.Data.DynamicData.BasePosition = UnitBasePosition;
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
    
    
    public enum SpawningStates
    {
        Default,
        CantSpawn,
        Turbo
    }


