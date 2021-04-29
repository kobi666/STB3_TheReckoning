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
        [TypeFilter("GetBatchStructures"), SerializeReference]
        public BatchStructure BatchStructure;

        public int WaveDelayInSpawnTimeMultiplier = 1;

        private static IEnumerable<Type> GetBatchStructures()
        {
            return UnitSpawning.BatchStructure.GetBatchStructures();
        }

        public void WaveInit()
        {
            BatchStructure.BatchInit();
        }

        public int AmountOfBatches = 1;
    }

    [Serializable]
    public abstract class BatchStructure
    {
        [ShowInInspector] public abstract bool[,] Formation { get; }

        public int FormationLength => Formation.GetLength(1);

        public static IEnumerable<Type> GetBatchStructures()
        {
            var q = typeof(BatchStructure).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(BatchStructure))); // Excludes classes not inheriting from BaseClass
            return q;
        }

        public bool[] GetColumn(int columnNumber)
        {
            return Enumerable.Range(0, Formation.GetLength(0))
                .Select(x => Formation[x, columnNumber])
                .ToArray();
        }

        public abstract void BatchInit();
    }

    public class ArrowShape : BatchStructure
    {
        [ShowInInspector]
        public override bool[,] Formation { get => formation; }
        public override void BatchInit()
        {
            formation[2, 0] = true;
            formation[1, 1] = true;
            formation[3, 1] = true;
            formation[0, 2] = true;
            formation[4, 2] = true;
        }

        private bool[,] formation = new bool[5, 3];
        

    }

    public class SingleUnit : BatchStructure
    {
        public override bool[,] Formation { get => formation; }
        public override void BatchInit()
        {
            formation[3, 1] = true;
        }
        private bool[,] formation = new bool[5,3];
    }
}


