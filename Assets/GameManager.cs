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

    private event Action<int> onLifeUpdate;

    public void OnLifeUpdate(int lifeDelta)
    {
        onLifeUpdate?.Invoke(lifeDelta);
    }
    
    

    public event Action onMoneyUpdate;

    public void OnMoneyUpdate()
    {
        onMoneyUpdate?.Invoke();
    }

    public void UpdateMoney(int Moneydelta)
    {
        CurrentLevelManager.ResourcesManager.Money += Moneydelta;
        OnMoneyUpdate();
    }

    public void UpdateLife(int LifeDelta)
    {
        if (!playerLost) {
        CurrentLevelManager.ResourcesManager.PlayerLife += LifeDelta;
        OnLifeUpdate(CurrentLevelManager.ResourcesManager.PlayerLife);
        if (CurrentLevelManager.ResourcesManager.PlayerLife <= 0)
            {
                playerLost = true;
                OnLose();
            }
        }
    }


    private bool playerLost = false;
    public event Action onLose;

    public void OnLose()
    {
        onLose?.Invoke();
    }
    
    [Required]
    public CanvasTextController UDedText;

    [Required] public CanvasTextController YouWinText;
    public bool FreeActions;

    public LevelManager CurrentLevelManager;
    
    [Required]
    public TextMeshProUGUI MoneyTextObject;

    [Required] public TextMeshProUGUI LifeTextObject;
    
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
        onMoneyUpdate += delegate { MoneyTextObject.text = CurrentLevelManager.ResourcesManager.Money.ToString(); };
        onLifeUpdate += delegate(int i) { LifeTextObject.text = CurrentLevelManager.ResourcesManager.PlayerLife.ToString();};
        onLose += UDedText.FadeTextInAndOut;
    }
    
    

    private void Start()
    {
        UpdateMoney(CurrentLevelManager.InitialMoney);
        UpdateLife(CurrentLevelManager.InitialLife);
    }
}
