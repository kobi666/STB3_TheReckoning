using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnitSpawning
{
    


    [Serializable]
    public class UnitWave
    {

        public float WaveStartupTime = 3f;
        
        [SerializeReference][TypeFilter("getBatchTypes")]
        public List<UnitBatchGroup> BatchGroup = new List<UnitBatchGroup>();
        
        public bool WaveFinished;
        private int batchCounter;

        public int BatchCounter
        {
            get => batchCounter;
            set
            {
                batchCounter = value;
                if (batchCounter > BatchGroup.Count)
                {
                    WaveFinished = true;
                }
            }
        }

        private IEnumerable<Type> getBatchTypes()
        {
            return UnitBatchGroup.GetBatchTypes();
        }

        public void InitWave()
        {
            foreach (var batch in BatchGroup)
            {
                batch.Init(this);
            }
        }

    }

    [Serializable]
    public abstract class UnitBatchGroup
    {
        private UnitWave ParentWave;
        public int AmountOfBatches = 5;
        public int TimeBetweenRowsMS = 500;
        
        [ShowInInspector]
        public abstract bool[,] BatchStructure { get; }
        
        public static IEnumerable<Type> GetBatchTypes()
        {
            var q = typeof(UnitBatchGroup).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(UnitBatchGroup))); // Excludes classes not inheriting from BaseClass
            return q;
        }
        
        [OnInspectorInit]
        public abstract void BatchInit();

        public void Init(UnitWave unitWave)
        {
            ParentWave = unitWave;
            BatchInit();
        }

        private int columnCounter;
        private bool batchFinished = false;
        private int batchCounter = 0;

        private bool allBatchesFinished;

        public bool AllBatchesFinished
        {
            get => allBatchesFinished;
            set
            {
                allBatchesFinished = value;
                if (allBatchesFinished)
                {
                    ParentWave.BatchCounter++;
                } 
            }
        }

        public int BatchCounter
        {
            get => batchCounter;
            set
            {
                batchCounter = value;
                if (batchCounter > AmountOfBatches)
                {
                    AllBatchesFinished = true;
                }
            }
        }

        public bool BatchFinished
        {
            get => batchFinished;
            set
            {
                if (value == true)
                {
                    batchFinished = value;
                    batchCounter++;
                }
            }
        }
        private int ColumnCounter
        {
            get => columnCounter;
            set
            {
                if (value > BatchStructure.GetLength(1) - 1)
                {
                    columnCounter = 0;
                    BatchFinished = true;
                }
                else
                {
                    columnCounter = value;    
                }
                
            }
        }

        public bool[] GetRow()
        {
            bool[] ba =  Enumerable.Range(0, BatchStructure.GetLength(0))
                .Select(x => BatchStructure[ x, ColumnCounter])
                .ToArray();
            
            
            if (!ba.IsNullOrEmpty())
            {
                ColumnCounter++;
            }

            return ba;
        }
    }

[Serializable]
public class ArrowShape : UnitBatchGroup
{
    
    public override bool[,] BatchStructure { get => batchStructure; }
    public override void BatchInit()
    {
        batchStructure[2, 0] = true;
        batchStructure[1, 1] = true;
        batchStructure[3, 1] = true;
        batchStructure[0, 2] = true;
        batchStructure[4, 2] = true;
    }
    
    

    private bool[,] batchStructure = new bool[5, 3];
}

public class SingleRandomSpline : UnitBatchGroup
{
    public override bool[,] BatchStructure { get; }
    public override void BatchInit()
    {
        
    }
} 
    

    
    
}    


