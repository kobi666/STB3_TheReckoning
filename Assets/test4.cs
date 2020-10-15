using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test4 : MonoBehaviour
{
    private ComponentRotator rot;
    void Start()
    {
        rot = GetComponent<ComponentRotator>();
        
        rot.StartAsyncRotationToweradsTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
