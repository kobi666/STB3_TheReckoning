using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class test4 : MonoBehaviour
{
	private ComponentRotator rot;
    
	
    void Start()
    {
        rot = GetComponent<ComponentRotator>();
        
	    rot.StartAsyncRotationToweradsTarget();
        
	    //transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
