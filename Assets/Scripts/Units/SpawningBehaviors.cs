using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using System.Threading.Tasks;

[Serializable]
    public class SpawnSingleUnitToBasePosition : SpawnerBehavior
    {
        public List<UnitPoolCreationData> unitCreationData = new List<UnitPoolCreationData>();
        public override List<UnitPoolCreationData> UnitCreationData { get => unitCreationData; }
        private PoolObjectQueue<GenericUnitController> UnitQueuePool;


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
            UnitQueuePool = UnitCreationData[0].CreateUnitPool();
            PathPointFinder.onPathFound += SetUnitBasePosition;
        }

        void SetUnitBasePosition()
        {
            UnitBasePosition = PathPointFinder.GetPathPointByPriority(PathPointTypesByPriority);
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


