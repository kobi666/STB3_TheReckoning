using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitSpawnBatch  {
    
    public rowData[] rows = new rowData[7]; //Grid of 7x7
}

[System.Serializable]
public struct rowData{
    public bool[] row;
}