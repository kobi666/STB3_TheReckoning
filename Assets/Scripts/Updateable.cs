using System;

public interface IDynamicallyUpdateable<T>
{
    event Action<T> onDynamicUpdate;

    void ApplyDynamicUpdate(T t);
    T DynamicUpdateInstance { get; set; }
}






