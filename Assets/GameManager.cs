using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using MyBox;

[DefaultExecutionOrder(-20)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Camera MainCamera;

    public ItemManager ItemManager;

    public List<LevelManager> AllLevels = new List<LevelManager>();
    
    private Queue<LevelManager> AllLevelsQueue = new Queue<LevelManager>();

    [Required] public PlayerTowersManager PlayerTowersManager;
    
    [Required]
    public SelectorTest2 PlayerCursor;

    private event Action<int> onLifeUpdate;

    public void OnLifeUpdate(int lifeDelta)
    {
        onLifeUpdate?.Invoke(lifeDelta);
    }

    public event Action onLevelVictoryAcheived;

    public void OnLevelVictoryAcheived()
    {
        onLevelVictoryAcheived?.Invoke();
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
        get => CurrentLevelManager?.ResourcesManager.Money ?? -999;
        set
        {
            CurrentLevelManager.ResourcesManager.Money += value;
            MoneyTextObject.text = Money.ToString();
        }
    }

    private LevelManager PreviousLevel;
    private void Awake()
    {
        //onCameraFinishedMoving += delegate { if (PreviousLevel != null) { PreviousLevel.gameObject.SetActive(false);} };
        Instance = this;
        MainCamera = Camera.main;
        onMoneyUpdate += delegate { MoneyTextObject.text = CurrentLevelManager.ResourcesManager.Money.ToString(); };
        onLifeUpdate += delegate(int i) { LifeTextObject.text = CurrentLevelManager.ResourcesManager.PlayerLife.ToString();};
        onLose += UDedText.FadeTextInAndOut;
        foreach (var level in AllLevels)
        {
            AllLevelsQueue.Enqueue(level);
        }

        ItemManager = GetComponent<ItemManager>();
        onLevelVictoryAcheived += StartVictoryTextSequenceAndChangeLevel;
    }

    public void StartNewLevel(LevelManager levelManager)
    {
        if (CurrentLevelManager != null)
        {
            CurrentLevelManager.AllUnitsFinished -= OnLevelVictoryAcheived;
            PreviousLevel = CurrentLevelManager;
        }
        
        onCameraFinishedMoving += DisablePreviousLevel;
        CurrentLevelManager = levelManager;
        levelManager.gameObject.SetActive(true);
        var levelPos = levelManager.transform.position;
        MoveCameraSmooth(1.5f, levelPos);
        UpdateMoney(CurrentLevelManager.InitialMoney);
        UpdateLife(CurrentLevelManager.InitialLife);
        CurrentLevelManager.AllUnitsFinished += OnLevelVictoryAcheived;
    }

    void DisablePreviousLevel()
    {
        if (PreviousLevel != null)
        {
            PreviousLevel.gameObject.SetActive(false);
        }
    }


    public void StartVictoryTextSequenceAndChangeLevel()
    {
        YouWinText.OnFadeOutEnd += DoStuffOnVictory;
        YouWinText.FadeTextInAndOut();
    }


    public event Action onGameVictory;
    public void DoStuffOnVictory()
    {
        if (AllLevelsQueue.Count > 0)
        {
            StartNewLevel(AllLevelsQueue.Dequeue());
            PlayerCursor.InitializeCursorForLevel();
        }
        else
        {
            onGameVictory?.Invoke();
        }
    }
    

    private event Action onCameraFinishedMoving;
    public bool MovementInProgress = false;
    public async void MoveCameraSmooth(float MovementDuration, Vector2 targetPos)
    {
        var cameraTransfrom = MainCamera.transform;
        float t = 0;
        MovementInProgress = true;
        var cameraZ = cameraTransfrom.position.z;
        float startTime = Time.time;
        while (t < 1 && MovementInProgress)
        {
            t = (Time.time - startTime) / MovementDuration;
            cameraTransfrom.position = new Vector3(Mathf.SmoothStep(cameraTransfrom.position.x, targetPos.x, t),Mathf.SmoothStep(cameraTransfrom.position.y, targetPos.y, t), cameraZ);
            await Task.Yield();
        }
        if (MovementInProgress) {
        onCameraFinishedMoving?.Invoke();
        }
        MovementInProgress = false;
    }

    private void OnDisable()
    {
        MovementInProgress = false;
    }

    

    private void Start()
    {
        StartNewLevel(AllLevelsQueue.Dequeue());
        /*UpdateMoney(CurrentLevelManager.InitialMoney);
        UpdateLife(CurrentLevelManager.InitialLife);*/
        PlayerCursor.InitializeCursorForLevel();
    }
}
