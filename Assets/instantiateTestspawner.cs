using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class instantiateTestspawner : MonoBehaviour
{
    public List<ProjectileMovementFunction> test1 = new List<ProjectileMovementFunction>();
    
    
    
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
