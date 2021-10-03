using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public PlayerInput.GamePlayActions GamePlayActions;
    public PlayerInput.ItemSelectionActions ItemSelectionActions;

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

    public event Action<ButtonDirectionsNames> onButtonHoldStart;

    public void OnButtonHoldStart(ButtonDirectionsNames buttonName)
    {
        onButtonHoldStart?.Invoke(buttonName);
    }

    private void Awake()
    {
        PlayerInput = new PlayerInput();
        GamePlayActions = PlayerInput.GamePlay;
        ItemSelectionActions = PlayerInput.ItemSelection;
    }

    private void OnEnable()
    {
        PlayerInput.GamePlay.Enable();
    }

    private void OnDisable()
    {
        PlayerInput.GamePlay.Disable();
        ItemSelectionActions.Disable();
    }

    private void Start()
    {
        GamePlayActions.NorthButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.North);
        GamePlayActions.EastButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.East);
        GamePlayActions.SouthButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.South);
        GamePlayActions.WestButton.performed += ctx => OnActionButtonPress(ButtonDirectionsNames.West);

        ItemSelectionActions.NorthButton.started += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {

            }
        };
    }
}
