using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitMover : MonoBehaviour
{
    public static UnitMover Instance;
    public event Action OnMove;
    private void Awake()
    {
        Instance = this;
        OnMove += DoNothing;
        
    }

    void DoNothing()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnMove.Invoke();
        
    }

    private void OnDisable()
    {
        
    }
}