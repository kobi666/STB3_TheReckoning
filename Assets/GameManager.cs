using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

[DefaultExecutionOrder(-20)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool FreeActions;

    public LevelManager CurrentLevelManager;
    
    [Required]
    public TextMeshProUGUI MoneyTextObject;
    
    [ShowInInspector]
    public int Money
    {
        get => CurrentLevelManager.ResourcesManager.Money;
        set
        {
            CurrentLevelManager.ResourcesManager.Money += value;
            MoneyTextObject.text = Money.ToString();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Money = Money;
    }
}
