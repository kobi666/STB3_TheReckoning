﻿using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using DegreeUtils;

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
    
    
    public enum SpawningStates
    {
        Default,
        CantSpawn,
        Turbo
    }

