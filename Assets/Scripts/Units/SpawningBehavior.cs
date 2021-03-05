using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

[Serializable]
    public abstract class SpawningBehavior : IHasEffects
    {
        public PathPointFinder PathPointFinder;
        
        public abstract List<UnitPoolCreationData> UnitCreationData { get; }

        public event Action onBehaviorStart;
        public event Action onBehaviorEnd;
        
        private bool externalSpawningLock;
        public bool ExternalSpawningLock
        {
            get => externalSpawningLock;
            set
            {
                externalSpawningLock = value;
                if (value == false)
                {
                    BehaviorInProgress = false;
                }
            }
        }
        public TowerComponent ParentComponent;

        public void InitBehavior(TowerComponent parentComponent,PathPointFinder pointFinder)
        {
            ParentComponent = parentComponent;
            PathPointFinder = pointFinder;
        }

        public abstract void SpecificBehaviorInit();

        public bool BehaviorInProgress = false;
        public async void InvokeBehavior()
        {
            if (BehaviorInProgress == false)
            {
                BehaviorInProgress = true;
                while (ExternalSpawningLock)
                {
                    Behavior();
                    await Task.Yield();
                }

                BehaviorInProgress = false;
            }
        }

        public abstract void Behavior();

        public abstract List<Effect> GetEffectList();


        public abstract void UpdateEffect(Effect ef, List<Effect> appliedEffects);


        public abstract void SetEffectList(List<Effect> effects);
        
        public static IEnumerable<Type> GetBehaviors()
        {
            var q = typeof(SpawningBehavior).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => x.IsSubclassOf(typeof(SpawningBehavior))); // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
