using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool FreeActions;

    public LevelManager CurrentLevelManager;

    private void Awake()
    {
        Instance = this;
    }
}
