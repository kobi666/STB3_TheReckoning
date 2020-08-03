using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectManager : MonoBehaviour
{
    public Dictionary<string, InGameObjectDictionary<Component>> Dictionaries = new Dictionary<string, InGameObjectDictionary<Component>>();

    public InGameObjectDictionary<Component> GetOrAddObjectDictionary(string ObjectTypeString) {
        InGameObjectDictionary<Component> dict = null;
        if (!Dictionaries.ContainsKey(ObjectTypeString)) {
            
        }

        return dict;
    }

}
