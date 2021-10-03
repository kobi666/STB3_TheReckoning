using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class VDtest : MonoBehaviour
{
    public TestBuffClass TestBuffClass;

    [Button]
    public void instatiateSelf()
    {
        GameObject go = GameObject.Instantiate(TestBuffClass.gameObject);
    }
    
    
    [Button]
    public void AddBuff()
    {
        TestBuffClass.someInt++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
