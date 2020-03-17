﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorTest2 : MonoBehaviour
{
    public GameObject H1;
    float FirstDiscoveryRange;
    public float SecondDiscoveryRangeMultiplier;
    public float SecondDiscoveryRange;
        
    
    public float DiscoveryRangeWithLineWidth;
    public float SecondDiscoveryRangeWithLineWidth;
    
    LineRenderer H1L;
    LineRenderer H2L;
    LineRenderer H3L;
    LineRenderer H4L;
    LineRenderer V1L;
    LineRenderer V2L;
    LineRenderer V3L;
    LineRenderer V4L;
    public GameObject H2;
    public GameObject H3;
    public GameObject H4;

    public GameObject V1;
    public GameObject V2;
    public GameObject V3;
    public GameObject V4;

    public PlayerInput PlayerControl;
    public float MoveLock;

    [SerializeField]
    GameObject selectedTowerSlot;
    TowerController TowerObjectController;

    public GameObject SelectedTowerSlot {
        get => selectedTowerSlot;
        set {
            selectedTowerSlot = value;
            SlotController = selectedTowerSlot?.GetComponent<TowerSlotController>() ?? null;
            TowerObject = SlotController.TowerObject;
            TowerObjectController = SlotController.TowerObjectController;
        }
    }

    public GameObject TowerObject;

    public TowerSlotController SlotController;
    public TowerSlotActions TowerActions { get => SlotController.TowerObjectController.TowerActions;}

    public Dictionary<Vector2, TowerUtils.TowerPositionData> CardinalTowerSlots {
        get => SlotController?.TowerSlotsByDirections8 ?? null;
    }

    public Dictionary<Vector2, GameObject> TowerSlotsWithPositions;
    public static SelectorTest2 instance;
    public float speed;
    Vector2 Move;

    private void OnEnable() {
        PlayerControl.GamePlay.Enable();
    }

    void resetMoveCounter() {
        MoveLock = 1.0f;
    }

    private void Awake() {
        
        instance = this;
        PlayerControl = new PlayerInput();
        TowerSlotsWithPositions = TowerUtils.TowerSlotsWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));
        

        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = ctx.ReadValue<Vector2>();
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => DebugAxis(ctx.ReadValue<Vector2>(), TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()));
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => MoveToNewTower4(TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>() ) );
        
        
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => resetMoveCounter();
        //PlayerControl.GamePlay.NorthButton.performed += ctx => TowerActions.ButtonNorth.ExecuteFunction(TowerSlotController.TowerSlot, TowerSlotController.gameObject);
        PlayerControl.GamePlay.NorthButton.performed += ctx => TowerActions.ButtonNorth.ExecuteFunction();
        PlayerControl.GamePlay.EastButton.performed += ctx => TowerActions.ButtonEast.ExecuteFunction();
        PlayerControl.GamePlay.SouthButton.performed += ctx => TowerActions.ButtonSouth.ExecuteFunction();
        PlayerControl.GamePlay.WestButton.performed += ctx => TowerActions.ButtonWest.ExecuteFunction();
    }





    public void DebugAxis(Vector2 real, Vector2 normalized ) {
        Debug.Log("Real :" + real + ",  Normalized : " + normalized);
    }



    public void MoveToNewTower4(Vector2 cardinalDirectionV2) {
        if (MoveLock >= 0.20f) {
            MoveLock = 0.0f;
            if (cardinalDirectionV2 == Vector2.zero) {
                Debug.Log("Didn't move cause Vector2 was ZERO");
                return;
            }
            GameObject towerSlotGO = null;
            if (CardinalTowerSlots.ContainsKey(cardinalDirectionV2)) {
                towerSlotGO = CardinalTowerSlots[cardinalDirectionV2].TowerSlotGo;
            }
            if (towerSlotGO != null) {
            transform.position = towerSlotGO.transform.position;
            SelectedTowerSlot = towerSlotGO;
            }

        }
        else {
            //Debug.Log("Move lock was less then 0.1");
        }
    }

    private void OnDisable() {
        PlayerControl.GamePlay.Disable();
        MoveLock = 0.7f;
        PlayerControl = new PlayerInput();
    }
    // Start is called before the first frame update
    void Start()
    {
        FirstDiscoveryRange = StaticObjects.instance.TowerSize;
        V1L = V1.GetComponent<LineRenderer>();
        V2L = V2.GetComponent<LineRenderer>();
        V3L = V3.GetComponent<LineRenderer>();
        V4L = V4.GetComponent<LineRenderer>();
        H1L = H1.GetComponent<LineRenderer>();
        H2L = H2.GetComponent<LineRenderer>();
        H3L = H3.GetComponent<LineRenderer>();
        H4L = H4.GetComponent<LineRenderer>();
        DiscoveryRangeWithLineWidth = FirstDiscoveryRange + 0.10f;
        int random = UnityEngine.Random.Range(1, TowerSlotsWithPositions.Count);
        SelectedTowerSlot = GameObject.FindGameObjectWithTag("TowerParent").transform.GetChild(random).gameObject;
        transform.position = SelectedTowerSlot.transform.position;
        SecondDiscoveryRange = FirstDiscoveryRange * SecondDiscoveryRangeMultiplier;
        SecondDiscoveryRangeWithLineWidth = SecondDiscoveryRange + 0.10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveLock < 1.0f) {
            MoveLock += Time.deltaTime;
        }
        H1L.SetPosition(0, new Vector2(-10, SelectedTowerSlot.transform.position.y + DiscoveryRangeWithLineWidth));
        H1L.SetPosition(1, new Vector2(10, SelectedTowerSlot.transform.position.y + DiscoveryRangeWithLineWidth));
        H2L.SetPosition(0, new Vector2(-10, SelectedTowerSlot.transform.position.y - DiscoveryRangeWithLineWidth));
        H2L.SetPosition(1, new Vector2(10, SelectedTowerSlot.transform.position.y - DiscoveryRangeWithLineWidth));
        H3L.SetPosition(0, new Vector2(-10, SelectedTowerSlot.transform.position.y + SecondDiscoveryRangeWithLineWidth));
        H3L.SetPosition(1, new Vector2(10, SelectedTowerSlot.transform.position.y + SecondDiscoveryRangeWithLineWidth));
        H4L.SetPosition(0, new Vector2(-10, SelectedTowerSlot.transform.position.y - SecondDiscoveryRangeWithLineWidth));
        H4L.SetPosition(1, new Vector2(10, SelectedTowerSlot.transform.position.y - SecondDiscoveryRangeWithLineWidth));
        V1L.SetPosition(0, new Vector2(SelectedTowerSlot.transform.position.x + DiscoveryRangeWithLineWidth, -6));
        V1L.SetPosition(1, new Vector2(SelectedTowerSlot.transform.position.x + DiscoveryRangeWithLineWidth, 6));
        V2L.SetPosition(0, new Vector2(SelectedTowerSlot.transform.position.x - DiscoveryRangeWithLineWidth, -6));
        V2L.SetPosition(1, new Vector2(SelectedTowerSlot.transform.position.x - DiscoveryRangeWithLineWidth, 6));
        V3L.SetPosition(0, new Vector2(SelectedTowerSlot.transform.position.x + SecondDiscoveryRangeWithLineWidth, -6));
        V3L.SetPosition(1, new Vector2(SelectedTowerSlot.transform.position.x + SecondDiscoveryRangeWithLineWidth, 6));
        V4L.SetPosition(0, new Vector2(SelectedTowerSlot.transform.position.x - SecondDiscoveryRangeWithLineWidth, -6));
        V4L.SetPosition(1, new Vector2(SelectedTowerSlot.transform.position.x - SecondDiscoveryRangeWithLineWidth, 6));
        
        
        

    }
}
