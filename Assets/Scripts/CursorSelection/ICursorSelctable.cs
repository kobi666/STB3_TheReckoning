using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICursorSelctable 
{
    CursorSelectablesManager CursorSelectablesManager { get; set; }
}


public abstract class CursorSelectableObjectData<T> where T : Component,ICursorSelctable
{
    [SerializeField]
    float distance;
    public float Distance {
        get => distance;
        set {
            distance = value;
        }
    }

    public int DiscoveryRangeCycleNumber = 99;

    public float ProximityScore(float discoveryRange)
    {
        return distance + (DiscoveryRangeCycleNumber * discoveryRange);
    }

    [SerializeField]
    T iCursorSelectable;
    
    [SerializeField]
    Vector2 towerPosition;
    public T IcCursorSelctable
    {
        get => iCursorSelectable;
        set => iCursorSelectable = value;

    }
    public Vector2 TowerPosition {
        get => towerPosition;
        set {
            towerPosition = value;
        }
    }

    public int ClockWiseIndex;
    

    public int CounterClockwiseIndex;
    

    
    

    public CursorSelectableObjectData(T tsc, float _distance)
    {
        if (tsc != null)
        {
            IcCursorSelctable = tsc;
        }

        Distance = _distance;
    }
    
    public CursorSelectableObjectData(T _towerGO, float _distance, int _cycleNumber) {
        IcCursorSelctable = _towerGO;
        DiscoveryRangeCycleNumber = _cycleNumber;
        if (_towerGO != null) {
            TowerPosition = (Vector2)_towerGO.transform.position;
        }
        Distance = _distance;
    }
    
    
}
