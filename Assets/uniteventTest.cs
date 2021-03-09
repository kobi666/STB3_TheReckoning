using UnityEngine;
using UnityEngine.Events;

public class uniteventTest : MonoBehaviour
{
    
    [System.Serializable]
    public class MyStringEvent : UnityEvent<string,string>
    {
        [SerializeField]
        public string someString;
    }

    public MyIntAction someAction;
    
    [System.Serializable]
    public class MyIntAction : UnityEvent<int>
    {
        
    }

    
    [System.Serializable]
    public class myFloatintstringAction : UnityEvent<float, string, int>
    {
        
    }

    public myFloatintstringAction somefloatintstring;

    public void testintfloatstring(float ff, string ss, int ii)
    {
        
    }


    public static void testIntFunc(int ii)
    {
        
    }
    
    
    public float f = 7f;
    public string s;
    
    
    [SerializeField]
    public  MyStringEvent TestAction;

    public static void PrintSomethingToconsole(float sss, string ss)
    {
        Debug.LogWarning(sss + ss);
    }

    private void Start()
    {
        somefloatintstring.AddListener(testintfloatstring);
    }
    
    
}
