/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class SelectorTest : MonoBehaviour
{
    public PlayerInput PlayerControl;
    public float MoveLock;
    
    
    [SerializeField]
    GameObject selectedTower;

    public GameObject SelectedTower {
        get => selectedTower;
        set {
            selectedTower = value;
            TowerController = value.GetComponent<TowerTestScript>() ?? null;
        }
    }

    Dictionary<Vector2, TowerPositionData> cardinalTowers = new Dictionary<Vector2, TowerPositionData>();
    public Dictionary<Vector2, TowerPositionData> CardinalTowers {
        get => cardinalTowers = TowerController.towersByDirections8 ?? null;
    }

    public void MoveToNewTower8(Vector2 cardinalDirectionV2, Vector2 NextClosestV2) {
        
        if (cardinalDirectionV2 == Vector2.zero) {
            return;
        }
        //Debug.Log(cardinalDirectionV2 + " : " + NextClosestV2);
        if (MoveLock >= 0.15f) {
            MoveLock = 0.0f;
            GameObject towerGO = CardinalTowers[cardinalDirectionV2].TowerSlotGo;
            GameObject AdjecentTower = null;
            if (cardinalDirectionV2 == Vector2.zero) {
                return;
            }
            if (towerGO != null) {
            
            transform.position = CardinalTowers[cardinalDirectionV2].TowerPosition;
            SelectedTower = CardinalTowers[cardinalDirectionV2].TowerSlotGo;
            }
            if (towerGO == null) {
                Vector2 CW = TowerUtils.DirectionsClockwise8[CardinalTowers[cardinalDirectionV2].ClockWiseIndex];
                Vector2 CCW = TowerUtils.DirectionsClockwise8[CardinalTowers[cardinalDirectionV2].CounterClockwiseIndex];
                //  float distanceClockWise = Vector2.Distance(CW, NextClosestV2);
                //  float DistanceCounterClockWise = Vector2.Distance(CCW, NextClosestV2);
                 float distanceClockWise = Vector2.Distance(CardinalTowers[CW].TowerPosition + CW, NextClosestV2 + (Vector2)transform.position);
                 float DistanceCounterClockWise = Vector2.Distance(CardinalTowers[CCW].TowerPosition + CCW, (Vector2)transform.position + NextClosestV2);
                if (distanceClockWise < DistanceCounterClockWise) {
                    if (CardinalTowers[CW].TowerSlotGo != null) {
                    
                    AdjecentTower = CardinalTowers[CW].TowerSlotGo;
                    }
                }
                else {
                    if (CardinalTowers[CCW].TowerSlotGo != null) {
                    AdjecentTower = CardinalTowers[CCW].TowerSlotGo;
                    }
                }
                if (AdjecentTower != null) {
                transform.position = AdjecentTower.transform.position;
                SelectedTower = AdjecentTower;
                }
            }
            
            
        }
        
    }

    public void MoveToNewTower4(Vector2 cardinalDirectionV2, Vector2 NextClosestV2) {
        
        if (cardinalDirectionV2 == Vector2.zero) {
            return;
        }
        //Debug.Log(cardinalDirectionV2 + " : " + NextClosestV2);
        if (MoveLock >= 0.15f) {
            MoveLock = 0.0f;
            GameObject towerGO = CardinalTowers[cardinalDirectionV2].TowerSlotGo;
            GameObject AdjecentTower = null;
            if (cardinalDirectionV2 == Vector2.zero) {
                return;
            }
            if (towerGO != null) {
            //StartCoroutine(SelectorUtils.SmoothMove(transform,CardinalTowers[cardinalDirectionV2].TowerPosition));
            //transform.position = CardinalTowers[cardinalDirectionV2].TowerPosition;
            SelectedTower = CardinalTowers[cardinalDirectionV2].TowerSlotGo;
            }
            if (towerGO == null) {
                Vector2 CW = TowerUtils.DirectionsClockwise4[CardinalTowers[cardinalDirectionV2].ClockWiseIndex];
                Vector2 CCW = TowerUtils.DirectionsClockwise4[CardinalTowers[cardinalDirectionV2].CounterClockwiseIndex];
                //  float distanceClockWise = Vector2.Distance(CW, NextClosestV2);
                //  float DistanceCounterClockWise = Vector2.Distance(CCW, NextClosestV2);
                 float distanceClockWise = Vector2.Distance(CardinalTowers[CW].TowerPosition + CW, NextClosestV2 + (Vector2)transform.position);
                 float DistanceCounterClockWise = Vector2.Distance(CardinalTowers[CCW].TowerPosition + CCW, (Vector2)transform.position + NextClosestV2);
                if (distanceClockWise < DistanceCounterClockWise) {
                    if (CardinalTowers[CW].TowerSlotGo != null) {
                    
                    AdjecentTower = CardinalTowers[CW].TowerSlotGo;
                    }
                }
                else {
                    if (CardinalTowers[CCW].TowerSlotGo != null) {
                    AdjecentTower = CardinalTowers[CCW].TowerSlotGo;
                    }
                }
                if (AdjecentTower != null) {
                //StartCoroutine(SelectorUtils.SmoothMove(transform,CardinalTowers[cardinalDirectionV2].TowerPosition));
                //transform.position = AdjecentTower.transform.position;
                SelectedTower = AdjecentTower;
                }
            }
            
            
        }
        
    }



    


    public TowerTestScript TowerController;


    public Dictionary<Vector2, GameObject> towersWithPositions;
    public static SelectorTest instance;

    public float speed;

    public void TestFunction() {
        Debug.Log("TESTT");
    }
    Vector2 Move;

    private void OnEnable() {
        PlayerControl.GamePlay.Enable();
    }

    private void OnDisable() {
        PlayerControl.GamePlay.Disable();
    }

    private void Awake() {
        MoveLock = 1.0f;
        PlayerControl = new PlayerInput();
        instance = this;
        towersWithPositions = TowerUtils.TowerSlotsWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));
        foreach (var item in towersWithPositions) {
//            Debug.Log(item.Value.name);
        }
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = ctx.ReadValue<Vector2>();
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => GetCardinalDirectionFromAxis(Move)
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => MoveToNewTower4(GetCardinalDirectionFromAxis(ctx.ReadValue<Vector2>()), ctx.ReadValue<Vector2>());
        //PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = GetCardinalDirectionFromAxis(Move);
        
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => Move = Vector2.zero;

        //PlayerControl.GamePlay.TestAction.performed += TestFunction;
    }
    private void Start() {
        int random = UnityEngine.Random.Range(1, towersWithPositions.Count);
        SelectedTower = GameObject.FindGameObjectWithTag("TowerParent").transform.GetChild(random).gameObject;
        transform.position = SelectedTower.transform.position;
    }

    

    public Vector2 GetCardinalDirectionFromAxis(Vector2 movementInput) {
        Vector2 NormalizedVector = new Vector2();
        if (movementInput.x > 0.4f) {
            NormalizedVector.x = 1;
        }
        if (movementInput.x < -0.4f) {
            NormalizedVector.x = -1;
        }
        if (movementInput.y > 0.4f) {
            NormalizedVector.y = 1;
        }
        if (movementInput.y < -0.4f) {
            NormalizedVector.y = -1;
        }
        if (!CardinalTowers.ContainsKey(NormalizedVector)) {
            NormalizedVector.x = 0;
            NormalizedVector.y = 0;
        }
        return NormalizedVector;
    }

    private void Update() {
        if (MoveLock < 1.0f) {
            MoveLock += Time.deltaTime;
        }
    }
    
    
}
*/
