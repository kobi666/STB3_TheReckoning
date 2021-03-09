using UnityEngine;
using System;

public class ACT : MonoBehaviour
{
    public static ACT instance;
    public event Action TB1;
    public event Action TB2;
    public event Action TB3;
    public PlayerInput playerInput;
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
        playerInput = new PlayerInput();
        playerInput.TestButtons.T.performed += ctx => TB1?.Invoke();
        playerInput.TestButtons.Y.performed += ctx => TB2?.Invoke();
        playerInput.TestButtons.U.performed += ctx => TB3?.Invoke();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        playerInput.TestButtons.Enable();
    }
}
