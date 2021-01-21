using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public interface IDynamicallyUpdateable<T>
{
    event Action<T> onDynamicUpdate;

    void ApplyDynamicUpdate(T t);
    T DynamicUpdateInstance { get; set; }
}






