using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingHold : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public TestingHold TestingHold;

    public void OnStarted()
    {
        Debug.LogWarning($"STARTED {Time.time}");
    }

    public void OnComplete()
    {
        Debug.LogWarning($"COMPLETE {Time.time}");
    }

    public void OnCancelled()
    {
        Debug.LogWarning($"CANCELLED {Time.time}");
    }
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TestingHold = new TestingHold();
        TestingHold.Test1.Enable();

        TestingHold.Test1.HoldAction.started += ctx => OnStarted();
        TestingHold.Test1.HoldAction.performed += ctx => OnComplete();
        TestingHold.Test1.HoldAction.canceled += ctx => OnCancelled();
    }

    private void OnDisable()
    {
        TestingHold.Test1.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
