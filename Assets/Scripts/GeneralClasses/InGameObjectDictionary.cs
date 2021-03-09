using System.Collections.Generic;
using UnityEngine;

public class InGameObjectDictionary<T> where T : Component
{
    public Dictionary <string,T> dictionary = new Dictionary<string, T>();
    
}
