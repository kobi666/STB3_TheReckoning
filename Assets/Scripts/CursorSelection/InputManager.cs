using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public PlayerInput.GamePlayActions GamePlayActions;

    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = Instantiate(new GameObject());
                go.name = "InputManager";
                instance = go.AddComponent<InputManager>();
                return instance;
            }

            return instance;
        }
    }

    public event Action<ButtonDirectionsNames> onActionButtonPress;

    public void OnActionButtonPress(ButtonDirectionsNames buttonName)
    {
        onActionButtonPress?.Invoke(buttonName);
    }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        GamePlayActions = PlayerInput.GamePlay;
    }

    private void OnEnable()
    {
        PlayerInput.GamePlay.Enable();
    }

    private void OnDisable()
    {
        PlayerInput.GamePlay.Disable();
    }

    private void Start()
    {
        GamePlayActions.NorthButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.North);
        GamePlayActions.EastButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.East);
        GamePlayActions.SouthButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.South);
        GamePlayActions.WestButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.West);
    }
}
