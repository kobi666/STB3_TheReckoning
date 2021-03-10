using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

    [Serializable]
    public class UnitWave
    {
        
        public float IntervalBetweenBatcheGroup;
        
        [SerializeReference][TypeFilter("getBatchTypes")]
        public List<UnitBatch> Batches = new List<UnitBatch>();
        
        private IEnumerable<Type> getBatchTypes()
        {
            return UnitBatch.GetBatchTypes();
        }
        
    }

    [Serializable]
    public abstract class UnitBatch
    {
        public int AmountOfBatches;
        public float TimeBetweenRows = 0.5f;
        
        [ShowInInspector]
        public abstract bool[,] BatchStructure { get; }
        
        public static IEnumerable<Type> GetBatchTypes()
        {
            var q = typeof(UnitBatch).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(UnitBatch))); // Excludes classes not inheriting from BaseClass
            return q;
        }
        
        [OnInspectorInit]
        public abstract void init();
    }

[Serializable]
public class ArrowShape : UnitBatch
{
    
    public override bool[,] BatchStructure { get => batchStructure; }
    public override void init()
    {
        batchStructure[2, 0] = true;
        batchStructure[1, 1] = true;
        batchStructure[3, 1] = true;
        batchStructure[0, 2] = true;
        batchStructure[4, 2] = true;
    }

    private bool[,] batchStructure = new bool[5, 3];

    



}
    

    
    
    


