using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;

    private void OnEnable()
    {
        Debug.LogWarning("OnEnable happened");
    }

    private void Awake()
    {
        Debug.LogWarning("Awake Happened");
    }

    private void Start()
    {
        Debug.LogWarning("StartHappened");
    }
}
