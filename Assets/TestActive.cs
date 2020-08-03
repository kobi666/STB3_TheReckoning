using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActive : MonoBehaviour,IActiveObject<TestActive>
{

    public ActiveObjectPool<TestActive> ActivePool { get => GameObjectPool.Instance.ActiveTest; set{}}
    // Start is called before the first frame update
    void Start()
    {
        ActivePool.AddObjectToActiveObjectPool(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
