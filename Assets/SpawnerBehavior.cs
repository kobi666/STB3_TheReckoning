using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;
using Sirenix.OdinInspector;

[Serializable]
    public abstract class SpawnerBehavior : IHasEffects
    {
        public PathPointFinder PathPointFinder;
        [HideInInspector] public Vector2? SpecifiedBasePosition = null;

        public List<PoolObjectQueue<GenericUnitController>> UnitPools;

        public float SpawnInterval = 5f;

        public event Action onPositionRecalculation;
        
        [Button]
        public void OnPositionRecalculation()
        {
            onPositionRecalculation?.Invoke();
        }

        public event Action onBehaviorStart;

        public void OnBehaviorStart()
        {
            onBehaviorStart?.Invoke();
        }
        public event Action onBehaviorEnd;
        
        private bool externalSpawningLock;
        public bool ExternalSpawningLock
        {
            get => externalSpawningLock;
            set
            {
                externalSpawningLock = value;
            }
        }
        public GenericUnitSpawner ParentComponent;

        public void InitBehavior(GenericUnitSpawner parentSpawner,PathPointFinder pointFinder)
        {
            ParentComponent = parentSpawner;
            PathPointFinder = pointFinder;
            PathPointFinder.onPathFound += OnPositionRecalculation;
            SpecificBehaviorInit();
            onBehaviorStart += InvokeBehavior;
            //ParentComponent.onManagedUnitRemoval += OnBehaviorStart;
        }

        public abstract void SpecificBehaviorInit();

        public float SpawnCounter;
        public bool BehaviorInProgress = false;
        
        
        [Button("Start SpawnerBehavior")]
        public async void InvokeBehavior()
        {
            try
            {
                if (BehaviorInProgress == false)
                {
                    BehaviorInProgress = true;
                    while (BehaviorInProgress)
                    {
                        if (ExecCondition())
                        {
                            SpawnCounter = Mathf.Clamp(SpawnCounter + StaticObjects.DeltaGameTime, 0f, SpawnInterval + 0.1f);
                            if (SpawnCounter >= SpawnInterval)
                            {
                                Behavior();
                                SpawnCounter = 0;
                            }
                        }
                    
                        await Task.Yield();
                    }

                    BehaviorInProgress = false;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                throw;
            }
            
        }

        private bool ExecCondition()
        {
            return !ExternalSpawningLock && BehaviorConditions();
        }

        public abstract bool BehaviorConditions();

        public abstract void Behavior();

        public abstract List<Effect> GetEffectList();
        


        public abstract void UpdateEffect(Effect ef);


        public abstract void SetEffectList(List<Effect> effects);
        
        public static IEnumerable<Type> GetBehaviors()
        {
            var q = typeof(SpawnerBehavior).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(SpawnerBehavior))); // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
