using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources instance;

    private TextMeshPro tmpro;
    public int StartupMoneyz;
    
    
    public int moneyz { get; private set; }
    public int chronoquilla { get; private set; }

    public event Action<int> updateMoneyz;
    public event Action<int> onMoneyzUpdate;

    public void UpdateMoneyz(int moneyzValue)
    {
        updateMoneyz?.Invoke(moneyzValue);
    }

    void UpdateTheMoneyz(int moneyzAmount)
    {
        moneyz += (moneyzAmount);
        onMoneyzUpdate?.Invoke(moneyz);
    }
    
    


    private void Awake()
    {
        tmpro = GetComponent<TextMeshPro>();
        instance = this;
        updateMoneyz += UpdateTheMoneyz;
        updateMoneyz += delegate(int i) { tmpro.text = moneyz.ToString(); };
    }

    
    
    private void Start()
    {
        UpdateTheMoneyz(StartupMoneyz); 
    }
}
