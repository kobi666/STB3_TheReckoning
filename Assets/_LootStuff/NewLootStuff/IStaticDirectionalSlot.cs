using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public abstract class StaticDirectionalSlot<T> : MyGameObject where T : StaticDirectionalSlot<T>
{


    [ShowInInspector][HideLabel][ReadOnly]
    private string a = "Neighbouring Slots";
    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/1",false)]
    public T NorthWestSlot;
    
    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/1",false)]
    public T WestSlot;
    
    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/1",false)]
    public T SouthWestSlot;
    
    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/2",false)]
    public T NorthSlot;

    [HorizontalGroup("Split", 120, LabelWidth = 120)] 
    [BoxGroup("Split/2", false)] [ShowInInspector]
    public bool Center;

    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/2",false)]
    public T SouthSlot;

    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/3",false)]
    public T NorthEastSlot;
    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/3",false)]
    public T EastSlot;

    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/3",false)]
    public T SouthEastSlot;
    
    [HorizontalGroup("Split", 120, LabelWidth = 120)]
    [BoxGroup("Split/1",false)]
    public Dictionary<Vector2,T> SlotsByNormalizedVector2 = new Dictionary<Vector2, T>();

    public abstract bool IsSlotAvailable();
    
    public T GetSlotByNormalizedVector(Vector2 normalizedVector)
    {
        if (normalizedVector == Vector2.up)
        {
            if (NorthSlot != null)
            {
                if (NorthSlot.IsSlotAvailable())
                {
                    return NorthSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.right)
        {
            if (EastSlot != null)
            {
                if (EastSlot.IsSlotAvailable())
                {
                    return EastSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.down)
        {
            if (SouthSlot != null)
            {
                if (SouthSlot.IsSlotAvailable())
                {
                    return SouthSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.left)
        {
            if (WestSlot != null)
            {
                if (WestSlot.IsSlotAvailable())
                {
                    return WestSlot;
                }    
            }
        }
        if (normalizedVector == (Vector2.up + Vector2.right))
        {
            if (NorthEastSlot != null)
            {
                if (NorthEastSlot.IsSlotAvailable())
                {
                    return NorthEastSlot;
                }    
            }
        }
        if (normalizedVector == (Vector2.down + Vector2.right))
        {
            if (SouthEastSlot != null)
            {
                if (SouthEastSlot.IsSlotAvailable())
                {
                    return SouthEastSlot;
                }    
            }
        }
        if (normalizedVector == (Vector2.down + Vector2.left))
        {
            if (SouthWestSlot != null)
            {
                if (SouthWestSlot.IsSlotAvailable())
                {
                    return SouthWestSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.up)
        {
            if (NorthSlot != null)
            {
                if (NorthSlot.IsSlotAvailable())
                {
                    return NorthSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.up)
        {
            if (NorthSlot != null)
            {
                if (NorthSlot.IsSlotAvailable())
                {
                    return NorthSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.up)
        {
            if (NorthSlot != null)
            {
                if (NorthSlot.IsSlotAvailable())
                {
                    return NorthSlot;
                }    
            }
        }
        if (normalizedVector == Vector2.up)
        {
            if (NorthSlot != null)
            {
                if (NorthSlot.IsSlotAvailable())
                {
                    return NorthSlot;
                }    
            }
        }
        return null;
    }

    
}
