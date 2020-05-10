using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEffect<T> {
    
    public void GetEffect(T t);
    public void AddtoEffectStack(string s, T t);

}


