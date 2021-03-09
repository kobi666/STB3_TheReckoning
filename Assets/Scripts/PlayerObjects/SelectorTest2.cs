using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectorTest2 : MonoBehaviour
{

    float FirstDiscoveryRange;
    public float SecondDiscoveryRangeMultiplier;
    public float SecondDiscoveryRange;
    public RangeDrawer RangeDrawer;
    private Shaker shaker;
    public float MovementDuration = 0.15f;
    public event Action<TowerSlotController> onTowerSelect;

    public void OnTowerSelect(TowerSlotController tsc)
    {
        onTowerSelect?.Invoke(tsc);
    }

    public bool MoveInProgress = false;
    public TowerPosIndicator[] indicators = new TowerPosIndicator[4];

    public PlayerInput PlayerControl;
    public float moveLock;

    [SerializeField]
    TowerSlotController selectedTowerSlot;
    TowerControllerLegacy SelectedTowerControllerLegacy {
        get => SelectedTowerSlot.towerObjectControllerLegacy;
    }

    public TowerSlotController SelectedTowerSlot {
        get => selectedTowerSlot;
        set {
            selectedTowerSlot = value;
            //SelectedTowerController.SlotController = SlotController;
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
        SelectedTowerControllerLegacy.TowerActions.ButtonNorth?.ExecuteFunction();
    }

    public void ExecEast() {
        
        SelectedTowerControllerLegacy.TowerActions.ButtonEast?.ExecuteFunction();
    }

    public void ExecSouth() {

        SelectedTowerControllerLegacy.TowerActions.ButtonSouth?.ExecuteFunction();
    }

    public void ExecWest() {
        
        SelectedTowerControllerLegacy.TowerActions.ButtonWest?.ExecuteFunction();
    }


    public TowerSlotActions TowerActions;

    public Dictionary<Vector2, TowerPositionData> CardinalTowerSlots {
        get => SelectedTowerSlot?.TowerSlotsByDirections8 ?? null;
    }

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

    void DrawRange(TowerSlotController tsc)
    {
        RangeDrawer.DrawCircle(SelectedTowerControllerLegacy?.mainTowerComponent?.Data.componentRadius ?? 0);
    }

    private void Awake()
    {
        shaker = GetComponent<Shaker>();
        instance = this;
        PlayerControl = new PlayerInput();
        /*TowerSlotsWithPositions =
            TowerUtils.TowerSlotsWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));*/
         
        onTowerSelect += SelectTowerSlot;
        onTowerSelect += DrawRange;
        onCantPerformActionDueToResources += Shake;
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = ctx.ReadValue<Vector2>();
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => DebugAxis(ctx.ReadValue<Vector2>(), TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()));
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx =>
            MoveToNewTower4(TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()));
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => ShowIndicators();

        
        
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


    public void ShowIndicators()
    {
        Transform[] towerTransforms = new Transform[4] {null, null, null, null};
        for (int i = 0; i < towerTransforms.Length; i++)
        {
            if (SelectedTowerSlot.TowersDebug[2 * i].GO != null)
            {
                towerTransforms[i] = SelectedTowerSlot.TowersDebug[2 * i].GO.transform;
                indicators[i].MoveToNewTarget( towerTransforms[i].position);
                indicators[i].SR.enabled = true;
                indicators[i].text.enabled = true;
                indicators[i].text.text = SelectedTowerSlot.TowersDebug[2 * i].Direction;
            }
            else
            {
                indicators[i].SR.enabled = false;
                indicators[i].text.enabled = false;
            }
        }

        

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



    public void MoveToNewTower4(Vector2 cardinalDirectionV2) {
        if (moveLock >= 0.20f) {
            moveLock = 0.0f;
            if (cardinalDirectionV2 == Vector2.zero) {
                Debug.Log("Didn't move cause Vector2 was ZERO");
                return;
            }
            
            TowerSlotController _towerSlotcontroller = null;
            if (CardinalTowerSlots.ContainsKey(cardinalDirectionV2)) {
                _towerSlotcontroller = CardinalTowerSlots[cardinalDirectionV2].TowerSlotController;
            }
            if (_towerSlotcontroller != null) {
            //transform.position = towerSlotGO.transform.position;
            OnTowerSelect(_towerSlotcontroller);
            
            StartCoroutine(SelectorUtils.SmoothMove(transform,_towerSlotcontroller.transform.position,0.3f,
                delegate { MoveInProgress = true; },  SetMoveInProgress));

            shaker.StopAllCoroutines();
            shaker.ShakeInProgress = false;
            MoveToNewTarget(_towerSlotcontroller.transform.position);
            
            
            //Debug.LogWarning("On Move : " + TowerObjectController.TowerActions.ButtonSouth.ActionDescription);


            //Debug.LogWarning(TowerActions.ButtonSouth.ActionDescription);
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
        TowerSlotParentManager.instance.OnGetTowerSlotsWithPositions();
        foreach (var tscv2 in TowerSlotParentManager.instance.TowerslotControllers.Values)
        {
            towerSlotsWithPositions.Add(tscv2.Item1,tscv2.Item2);
        }

        if (Legacy)
        {
            PlayerControl.GamePlay.NorthButton.performed += ctx =>
                ExecActionIfPossible(SelectedTowerControllerLegacy.TowerActions.ButtonNorth);
            PlayerControl.GamePlay.EastButton.performed += ctx =>
                ExecActionIfPossible(SelectedTowerControllerLegacy.TowerActions.ButtonEast);
            PlayerControl.GamePlay.SouthButton.performed += ctx =>
                ExecActionIfPossible(SelectedTowerControllerLegacy.TowerActions.ButtonSouth);
            PlayerControl.GamePlay.WestButton.performed += ctx =>
                ExecActionIfPossible(SelectedTowerControllerLegacy.TowerActions.ButtonWest);
        }

        if (!Legacy)
        {
            PlayerControl.GamePlay.NorthButton.performed +=
                ctx => SelectedTowerSlot.TowerActions.North.ExecuteAction(SelectedTowerSlot);
            PlayerControl.GamePlay.EastButton.performed +=
                ctx => SelectedTowerSlot.TowerActions.East.ExecuteAction(SelectedTowerSlot);
            PlayerControl.GamePlay.SouthButton.performed +=
                ctx => SelectedTowerSlot.TowerActions.South.ExecuteAction(SelectedTowerSlot);
            PlayerControl.GamePlay.WestButton.performed +=
                ctx => SelectedTowerSlot.TowerActions.West.ExecuteAction(SelectedTowerSlot);
        }

        Vector2 FirstKey = Vector2.zero;
        foreach (var item in towerSlotsWithPositions)
        {
            if (item.Value != null) {
            FirstKey = item.Key;
            break;
            }
            
        }
        FirstDiscoveryRange = StaticObjects.Instance.TowerSize;
        int random = UnityEngine.Random.Range(1, towerSlotsWithPositions.Count);
        OnTowerSelect(towerSlotsWithPositions[FirstKey]);
        //SelectedTowerSlot = towerSlotsWithPositions[FirstKey];
        MoveToNewTower4(SelectedTowerSlot.transform.position);
        //transform.position = SelectedTowerSlot.transform.position;
        SecondDiscoveryRange = FirstDiscoveryRange * SecondDiscoveryRangeMultiplier;

        //StartCoroutine(SelectorUtils.Shake(transform, 10f, 1.5f));
    }

    

    // Update is called once per frame
    void Update()
    {
        if (moveLock < 1.0f) {
            moveLock += Time.deltaTime;
        }

    }
}
