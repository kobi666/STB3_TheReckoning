using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public static ItemManager instance;

    private int _buffID = 0;

    public int BuffID
    {
        get
        {
            _buffID++;
            return _buffID;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    
}
