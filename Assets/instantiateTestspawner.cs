using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Security.Cryptography;
using System.Runtime.Serialization;

public class instantiateTestspawner : MonoBehaviour
{
    [System.Serializable]
    public class testClass
    {
        public int i;
        public string s;
    }
    
    

    public testClass uno = new testClass();
    public testClass dos = new testClass();


    [Button]
    public void getHashOfUno()
    {
        Debug.LogWarning(uno.GetHashCode());
    }

    [Button]
    public void getHashOfDos()
    {
        Debug.LogWarning(dos.GetHashCode());
    }
    
    
    
    public GameObject prefab;
    public int i = 0;
    private string md5;
    void Start()
    {
        
    }


    [Button]
    void asdasd()
    {
        Debug.LogWarning(GetHashCode());
    }
    
    void insadas()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
