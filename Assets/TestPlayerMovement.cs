using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : PlayerController
{
    PlayerInput input;
    Vector2 movement;
    
    
    private void OnEnable() {
        input.GamePlay.Enable();
    }
    private void Awake() {
        input = new PlayerInput();
        input.GamePlay.MovePlayer.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.GamePlay.MovePlayer.canceled += delegate { movement = Vector2.zero; } ;

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movement != Vector2.zero) {
        PlayerControllerUtilis.MoveInDirection(transform, movement, Data.MovementSpeed);
        }
    }
}
