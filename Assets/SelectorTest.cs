using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class SelectorTest : MonoBehaviour
{
    public PlayerInput PlayerControl;
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
        PlayerControl = new PlayerInput();
        instance = this;
        towersWithPositions = TowerUtils.TowersWithPositionsFromParent(GameObject.FindGameObjectWithTag("TowerParent"));
        foreach (var item in towersWithPositions) {
//            Debug.Log(item.Value.name);
        }
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = ctx.ReadValue<Vector2>();
        PlayerControl.GamePlay.MoveTowerCursor.performed += ctx => Move = GetCardinalDirectionFromAxis(Move);
        
        PlayerControl.GamePlay.MoveTowerCursor.canceled += ctx => Move = Vector2.zero;

        //PlayerControl.GamePlay.TestAction.performed += TestFunction;
    }
    private void Start() {
        
        
    }

    void NormalizeValueFromMovement(Vector2 movementInput) {
        
        float Nx = movementInput.x * 1.00f;
        float Ny = movementInput.y * 1.00f;
        Debug.Log(Nx + " " + Ny);
    }

    public Vector2 GetCardinalDirectionFromAxis(Vector2 movementInput) {
        Vector2 NormalizedVector = new Vector2();
        if (movementInput.x > 0.2f) {
            NormalizedVector.x = 1;
        }
        if (movementInput.x < -0.2f) {
            NormalizedVector.x = -1;
        }
        if (movementInput.y > 0.4f) {
            NormalizedVector.y = 1;
        }
        if (movementInput.y < -0.4f) {
            NormalizedVector.y = -1;
        }
        return NormalizedVector;
    }

    private void Update() {
        Vector2 m = new Vector2 (Move.x, Move.y) * Time.deltaTime * speed;
        transform.Translate(m, Space.World);
    }
    
    
}
