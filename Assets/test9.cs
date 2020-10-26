using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Object = System.Object;

public class test9 : MonoBehaviour
{
    [ShowInInspector,TypeFilter("GetFilteredTypeList")]
    public int someMono;

    [ShowInInspector,TypeFilter("GetFilteredTypeList")]
    public List<int> someintlist;
    
    
    private static IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(int).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(int).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass
        
        return q;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
