using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectorTest2 : MonoBehaviour
{

    float FirstDiscoveryRange;
    public float SecondDiscoveryRangeMultiplier;
    public float SecondDiscoveryRange;


    public TowerPosIndicator[] indicators = new TowerPosIndicator[4];
    /*public float DiscoveryRangeWithLineWidth;
    public float SecondDiscoveryRangeWithLineWidth;*/
    
    /*LineRenderer H1L;
    LineRenderer H2L;
    LineRenderer H3L;
    LineRenderer H4L;
    LineRenderer V1L;
    LineRenderer V2L;
    LineRenderer V3L;
    LineRenderer V4L;
    public GameObject H1;
    public GameObject H2;
    public GameObject H3;
    public GameObject H4;

    public GameObject V1;
    public GameObject V2;
    public GameObject V3;
    public GameObject V4;*/

    public PlayerInput PlayerControl;
    public float moveLock;

    [SerializeField]
    GameObject selectedTowerSlot;
    TowerController TowerObjectController {
        get => SlotController.TowerObjectController;
    }

    public GameObject SelectedTowerSlot {
        get => selectedTowerSlot;
        set {
            selectedTowerSlot = value;
            TowerObjectController.SlotController = SlotController;
//            Debug.LogWarning("On Set : " + TowerObjectController.TowerActions.ButtonSouth.ActionDescription);
        }
    }

    

    



    public void ExecNorth() {
        TowerObjectController.TowerActions.ButtonNorth?.ExecuteFunction();
    }

    public void ExecEast() {
        
        TowerObjectController.TowerActions.ButtonEast?.ExecuteFunction();
    }

    public void ExecSouth() {

        TowerObjectController.TowerActions.ButtonSouth?.ExecuteFunction();
    }

    public void ExecWest() {
        
        TowerObjectController.TowerActions.ButtonWest?.ExecuteFunction();
    }

    public GameObject TowerObject;

    public TowerSlotController SlotController {
        get => SelectedTowerSlot.GetComponent<TowerSlotController>();
    }
    public TowerSlotActions TowerActions;

    public Dictionary<Vector2, TowerPositionData> CardinalTowerSlots {
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
        moveLock = 1.0f;
    }

    private void Awake() {
        
        instance = this;
        PlayerControl = new PlayerInput();
        TowerSlotsWithPositions = TowerUtils.TowerSlotsWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));
        

        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = ctx.ReadValue<Vector2>();
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => DebugAxis(ctx.ReadValue<Vector2>(), TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()));
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => MoveToNewTower4(TowerUtils.GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>() ) );
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => ShowIndicators();
        
        
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => resetMoveCounter();
        //PlayerControl.GamePlay.NorthButton.performed += ctx => TowerActions.ButtonNorth.ExecuteFunction(TowerSlotController.TowerSlot, TowerSlotController.gameObject);
        
    }


    public void ShowIndicators()
    {
        Transform[] towerTransforms = new Transform[4] {null, null, null, null};
        for (int i = 0; i < towerTransforms.Length; i++)
        {
            if (SlotController.TowersDebug[2 * i].GO != null)
            {
                towerTransforms[i] = SlotController.TowersDebug[2 * i].GO.transform;
                indicators[i].MoveToNewTarget( towerTransforms[i].position);
                indicators[i].SR.enabled = true;
                indicators[i].text.enabled = true;
                indicators[i].text.text = SlotController.TowersDebug[2 * i].Direction;
            }
            else
            {
                indicators[i].SR.enabled = false;
                indicators[i].text.enabled = false;
            }
        }

        

    }




    public void DebugAxis(Vector2 real, Vector2 normalized ) {
        Debug.Log("Real :" + real + ",  Normalized : " + normalized);
    }



    public void MoveToNewTower4(Vector2 cardinalDirectionV2) {
        if (moveLock >= 0.20f) {
            moveLock = 0.0f;
            if (cardinalDirectionV2 == Vector2.zero) {
                Debug.Log("Didn't move cause Vector2 was ZERO");
                return;
            }
            GameObject towerSlotGO = null;
            if (CardinalTowerSlots.ContainsKey(cardinalDirectionV2)) {
                towerSlotGO = CardinalTowerSlots[cardinalDirectionV2].TowerSlotGo;
            }
            if (towerSlotGO != null) {
            //transform.position = towerSlotGO.transform.position;
            SelectedTowerSlot = towerSlotGO;
            StartCoroutine(SelectorUtils.SmoothMove(transform,towerSlotGO.transform.position,0.3f));
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
    // Start is called before the first frame update
    void Start()
    {
        PlayerControl.GamePlay.NorthButton.performed += ctx => ExecNorth();
        PlayerControl.GamePlay.EastButton.performed += ctx =>  ExecEast();
        PlayerControl.GamePlay.SouthButton.performed += ctx => ExecSouth();
        PlayerControl.GamePlay.WestButton.performed += ctx => ExecWest();
        Vector2 FirstKey = Vector2.zero;
        foreach (var item in TowerSlotsWithPositions)
        {
            FirstKey = item.Key;
            break;
        }
        FirstDiscoveryRange = StaticObjects.instance.TowerSize;
        int random = UnityEngine.Random.Range(1, TowerSlotsWithPositions.Count);
        SelectedTowerSlot = TowerSlotsWithPositions[FirstKey];
        transform.position = SelectedTowerSlot.transform.position;
        SecondDiscoveryRange = FirstDiscoveryRange * SecondDiscoveryRangeMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLock < 1.0f) {
            moveLock += Time.deltaTime;
        }

    }
}
