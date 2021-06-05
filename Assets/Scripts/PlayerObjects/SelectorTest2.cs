using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using MyBox;


[System.Serializable][DefaultExecutionOrder(20)]
public class SelectorTest2 : MonoBehaviour
{
    public LevelManager CurrentLevelManager;
    float FirstDiscoveryRange;
    public float SecondDiscoveryRangeMultiplier;
    public float SecondDiscoveryRange;
    public RangeDrawer RangeDrawer;
    private Shaker shaker;
    public float MovementDuration = 0.15f;

    public CursorActionsController CursorActionsController;
    

    public float ActionImageDistance = 1f;
    public event Action<TowerSlotController> onTowerSelect;

    public void OnTowerSelect(TowerSlotController tsc)
    {
        onTowerSelect?.Invoke(tsc);
    }

    public bool MoveInProgress = false;
    public TowerPosIndicator[] indicators = new TowerPosIndicator[4];
    private Dictionary<Vector2,TowerPosIndicator> indicatorsDict = new Dictionary<Vector2, TowerPosIndicator>();
    public TowerActionIndicator[] ActionIndicators = new TowerActionIndicator[4];
    private Dictionary<string,TowerActionIndicator> actionIndicatorsDict = new Dictionary<string, TowerActionIndicator>();

    public PlayerInput PlayerControl;
    public float moveLock;

    [SerializeField]
    TowerSlotController selectedTowerSlot;

    public TowerSlotController SelectedTowerSlot {
        get => selectedTowerSlot;
        set {
            selectedTowerSlot = value;
            CursorActionsController.UpdateTowerActions(value.TowerActions);
        }
    }
    
    public bool ExternalTowerSlotActionsLock = false;
    public bool CanPerformActions()
    {
        if (ExternalTowerSlotActionsLock == false)
        {
            return true;
        }

        return false;
    }

    void ExecActionIfPossible(TowerSlotAction towerAction)
    {
        if (towerAction == null)
        {
            return;
        }
        if (CanPerformActions() == true) {
            if (PlayerResources.instance.moneyz >= towerAction.ActionCost)
            {
                PlayerResources.instance.UpdateMoneyz(-(towerAction.ActionCost));
                towerAction.ExecuteFunction();
            }
            else
            {
                OnCantPerformActionDueToResources();
            }
        }
    }

    public event Action onCantPerformActionDueToResources;

    public void OnCantPerformActionDueToResources()
    {
        onCantPerformActionDueToResources?.Invoke();
    }

    
    



    public void ExecNorth() {
        
    }

    public void ExecEast() {
        
        
    }

    public void ExecSouth() {

       
    }

    public void ExecWest() {
        
    }


    public TowerSlotActions TowerActions;
    
    [ShowInInspector]
    public Dictionary<Vector2, TowerSlotController> CardinalTowerSlots {
        get => SelectedTowerSlot?.FoundTowerSlots ?? null;
    }
    
    
    [ShowInInspector]
    public Dictionary<Vector2, TowerSlotController> towerSlotsWithPositions = new Dictionary<Vector2, TowerSlotController>();
    public static SelectorTest2 instance;
    public float speed;
    Vector2 Move;

    private void OnEnable() {
        PlayerControl.GamePlay.Enable();
    }

    void resetMoveCounter() {
        moveLock = 1.0f;
    }

    void SelectTowerSlot(TowerSlotController tsc)
    {
        SelectedTowerSlot = tsc;
    }

    private void Awake()
    {
        CursorActionsController = GetComponent<CursorActionsController>();
        shaker = GetComponent<Shaker>();
        instance = this;
        PlayerControl = new PlayerInput();
        /*TowerSlotsWithPositions =
            TowerUtils.TowerSlotsWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));*/
         
        onTowerSelect += SelectTowerSlot;
        onCantPerformActionDueToResources += Shake;
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = ctx.ReadValue<Vector2>();
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => DebugAxis(ctx.ReadValue<Vector2>(), TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()));
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx =>
            MoveToNewTower4(TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()));
        

        
        
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => resetMoveCounter();
        //PlayerControl.GamePlay.NorthButton.performed += ctx => TowerActions.ButtonNorth.ExecuteFunction(TowerSlotController.TowerSlot, TowerSlotController.gameObject);
        
    }

    private void Shake()
    {
        if (MoveInProgress == false)
        {
            shaker.BeginShaking();
        }
    }


    public void ShowIndicatorsStupid(Dictionary<Vector2, TowerSlotController> slotsDict)
    {
        for (int i = 0; i < TowerUtils.DirectionsClockwise4.Length; i++)
        {
            TowerSlotController tsc = slotsDict[TowerUtils.DirectionsClockwise4[i]] != null
                ? slotsDict[TowerUtils.DirectionsClockwise4[i]]
                : null;
            Vector2 targetPos = tsc != null ? tsc.transform.position : SelectedTowerSlot.transform.position;
            indicatorsDict[TowerUtils.DirectionsClockwise4[i]].SR.enabled = tsc != null;
            indicatorsDict[TowerUtils.DirectionsClockwise4[i]].MoveToNewTarget(targetPos, tsc);
        }
    }
    
    
    
    public void ShowIndicators()
    {
        foreach (var directionToSlot in SelectedTowerSlot.FoundTowerSlots)
        {
            if (directionToSlot.Value != null)
            {
                indicatorsDict[directionToSlot.Key].SR.enabled = true;
                indicatorsDict[directionToSlot.Key].MoveToNewTarget(directionToSlot.Value.transform.position, directionToSlot.Value);
            }
            else
            {
                indicatorsDict[directionToSlot.Key].SR.enabled = false;
                indicatorsDict[directionToSlot.Key].TargetTowerSlot = null;
            }
        }
        
        
        
        /*foreach (var towerSlot in SelectedTowerSlot.FoundTowerSlots)
        {
            if (towerSlot.Value != null)
            {
                if (indicatorsDict.ContainsKey(towerSlot.Key)) {
                indicatorsDict[towerSlot.Key].MoveToNewTarget(towerSlot.Value.transform.position);
                indicatorsDict[towerSlot.Key].SR.enabled = true;
                }
            }
            else
            {
                if (indicatorsDict.ContainsKey(towerSlot.Key))
                {
                    indicatorsDict[towerSlot.Key].SR.enabled = false;
                }
            }
        }*/
        
        //write code to make redundednt indicators disapper
        
    }

    public void SetMoveInProgress()
    {
        MoveInProgress = false;
    }


    public void DebugAxis(Vector2 real, Vector2 normalized ) {
        Debug.Log("Real :" + real + ",  Normalized : " + normalized);
    }
    IEnumerator moveCoroutine;
    public void MoveToNewTarget(Vector2 TargetPos)
    {
        
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = SelectorUtils.SmoothMove(transform, TargetPos, MovementDuration, delegate { MoveInProgress = true; },
            delegate { MoveInProgress = false; });
        StartCoroutine(moveCoroutine);
    }


    public event Action onMovementComplete;
    [HideInInspector]
    public bool asyncMoveLock;
    public async void MoveToNewTargetAsync(Vector2 targetPos)
    {
        if (asyncMoveLock == false)
        {
            asyncMoveLock = true;
            float startTime = Time.time;
            float t;
            while (asyncMoveLock && (Vector2)transform.position != targetPos)
            {
                t = (Time.time - startTime) / MovementDuration;
                transform.position = new Vector2(Mathf.SmoothStep(transform.position.x, targetPos.x, t),Mathf.SmoothStep(transform.position.y, targetPos.y, t));
                await Task.Yield();
            }
            asyncMoveLock = false;
        }
        onMovementComplete?.Invoke();
    }
    
    



    public void MoveToNewTower4(Vector2 cardinalDirectionV2) {
        if (!asyncMoveLock) {
            if (moveLock >= 0.10f) {
                moveLock = 0.0f;
                if (cardinalDirectionV2 == Vector2.zero) {
                    Debug.Log("Didn't move cause Vector2 was ZERO");
                    return;
                }
                
                TowerSlotController _towerSlotcontroller = null;
                if (CardinalTowerSlots.ContainsKey(cardinalDirectionV2)) {
                    _towerSlotcontroller = CardinalTowerSlots[cardinalDirectionV2];
                }
                if (_towerSlotcontroller != null) {
                //transform.position = towerSlotGO.transform.position;
                
                
                /*StartCoroutine(SelectorUtils.SmoothMove(transform,_towerSlotcontroller.transform.position,0.3f,
                    delegate { MoveInProgress = true; },  SetMoveInProgress));*/

                shaker.StopAllCoroutines();
                shaker.ShakeInProgress = false;
                MoveToNewTargetAsync(_towerSlotcontroller.transform.position);
                ShowIndicatorsStupid(_towerSlotcontroller.FoundTowerSlots);
                OnTowerSelect(_towerSlotcontroller);
                
                //Debug.LogWarning("On Move : " + TowerObjectController.TowerActions.ButtonSouth.ActionDescription);


                //Debug.LogWarning(TowerActions.ButtonSouth.ActionDescription);
                }

            }
        }
        else {
            //Debug.Log("Move lock was less then 0.1");
        }
    }

    private void OnDisable() {
        PlayerControl.GamePlay.Disable();
        moveLock = 0.7f;
        PlayerControl = new PlayerInput();
    }
    
    public bool Legacy = false;

    // Start is called before the first frame update
    void Start()
    {
        int indicatorsCounter = 0;
        foreach (var direction in CardinalSet.Cardinal4.directionsClockwise)
        {
            indicatorsDict.Add(direction,indicators[indicatorsCounter]);
            indicatorsCounter++;
        }

        
        
        CurrentLevelManager = GameManager.Instance.CurrentLevelManager;
        /*foreach (var v2tsc in CurrentLevelManager.LevelTowerSlots.Values)
        {
            towerSlotsWithPositions.Add(v2tsc.Item1,v2tsc.Item2);
        }*/

        

        /*Vector2 FirstKey = Vector2.zero;
        foreach (var item in towerSlotsWithPositions)
        {
            if (item.Value != null) {
            FirstKey = item.Key;
            break;
            }
        }
        //FirstDiscoveryRange = StaticObjects.Instance.TowerSize;
        //int random = UnityEngine.Random.Range(1, towerSlotsWithPositions.Count);
        //SelectedTowerSlot = towerSlotsWithPositions[FirstKey];
        MoveToNewTower4(SelectedTowerSlot.transform.position);*/
        //transform.position = SelectedTowerSlot.transform.position;
       

        //StartCoroutine(SelectorUtils.Shake(transform, 10f, 1.5f));
    }

    public void InitializeCursorForLevel()
    {
        towerSlotsWithPositions.Clear();
        CurrentLevelManager = GameManager.Instance.CurrentLevelManager;
        foreach (var v2tsc in CurrentLevelManager.LevelTowerSlots.Values)
        {
            towerSlotsWithPositions.Add(v2tsc.Item1,v2tsc.Item2);
        }
        
        //Get First Tower
        float DistanceFromCenter = 999f;
        var levelPos = CurrentLevelManager.transform.position;
        Vector2 towerKey = new Vector2(9999f,9999f);
        foreach (var positionToTower in towerSlotsWithPositions)
        {
            float currentDistance = Vector2.Distance(levelPos, positionToTower.Key);
            if (currentDistance < DistanceFromCenter)
            {
                towerKey = positionToTower.Key;
            }
        }

        var foundCenterTower = towerSlotsWithPositions.ContainsKey(towerKey) ? towerSlotsWithPositions[towerKey] : null;
        
        if (foundCenterTower != null) {
        OnTowerSelect(foundCenterTower);
        }
        MoveToNewTower4(SelectedTowerSlot.transform.position);
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (moveLock < 1.0f) {
            moveLock += Time.deltaTime;
        }

    }
}
