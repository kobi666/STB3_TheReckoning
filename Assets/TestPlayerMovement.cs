using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    PlayerInput input;
    
    private void OnEnable() {
        input.GamePlay.Enable();
    }
    private void Awake() {
        input = new PlayerInput();
        input.GamePlay.MovePlayer.performed += ctx => Debug.LogWarning("Got Input");
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
