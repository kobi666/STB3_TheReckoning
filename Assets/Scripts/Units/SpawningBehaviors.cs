using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

///public classes used in this class are PathPointFinder : ParentComponent
/// 


namespace SpawningBehaviors
{
    [Serializable]
    public class SpawnSingleUnitToBasePosition : SpawningBehavior
    {
        public List<UnitPoolCreationData> unitCreationData = new List<UnitPoolCreationData>();
        public override List<UnitPoolCreationData> UnitCreationData { get => unitCreationData; }
        private PoolObjectQueue<GenericUnitController> UnitQueuePool;
        public int MaxUnits = 3;
        public Vector2? UnitBasePosition;
        public List<GenericUnitController> ManagedUnits;
        public float SpawningCounter;
        public float SpawnInterval = 4f;

        public bool SpawnMaxUnitsOnStartup;
        public override void SpecificBehaviorInit()
        {
            UnitQueuePool = UnitCreationData[0].CreateUnitPool();
        }

        public override void Behavior()
        {
            throw new NotImplementedException();
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
}

