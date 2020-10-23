using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Sirenix.OdinInspector;

public class test8 : MonoBehaviour
{
    [ShowInInspector]
    public Damage d = new Damage();
    
    
    private Vector2 tt = Vector2.zero;
    async void testfunc()
    {
        tt = new Vector2(5f,5f);
        while (tt != Vector2.zero)
        {
            transform.position = Vector2.MoveTowards(transform.position, tt, 2 * Time.deltaTime);
            tt += (Vector2.up * Time.deltaTime) * 0.2f;
            await Task.Yield();
        }
    }

    
   
     
    
    void Start()
    {
        testfunc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
