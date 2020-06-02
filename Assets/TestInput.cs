using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestInput : MonoBehaviour
{
    public static TestInput instance;
    PlayerInput playerInput;
    // Start is called before the first frame update
    public event Action onW;
    public void OnW() {
        onW?.Invoke();
    }

    public event Action onD;
    public void OnD(){
        onD?.Invoke();
    }
    
    void Start()
    {
        
        instance = this;
        playerInput.TestButtons.W.performed += ctx => OnW();
        playerInput.TestButtons.D.performed += ctx => OnD();

    }

    private void Awake() {
        playerInput = new PlayerInput();
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
