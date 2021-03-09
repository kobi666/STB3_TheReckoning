using MyBox;
using UnityEngine;

public class testclass : MonoBehaviour
{
    [System.Serializable]
    public class MyClass
    {
        public string t1;
        public int i1;
    }

    public bool bool1;

    
    [ConditionalField("bool1")]
    public MyClass data;

}
