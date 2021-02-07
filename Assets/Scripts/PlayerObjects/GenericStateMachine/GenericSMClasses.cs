using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectState<T>
{
    public event Action<T> onStateEnterActions;
    public event Action<T> onExitStateActions;
    public event Action<T> inStateActions;

    public event Predicate<T> stateExitConditions;
}
