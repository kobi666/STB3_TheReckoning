// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""c6057c45-71e0-4285-8e36-ed78742c4d27"",
            ""actions"": [
                {
                    ""name"": ""MoveTowerCursor"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e76c0444-f393-46fb-85f1-395fc048084e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TestAction"",
                    ""type"": ""Button"",
                    ""id"": ""9dfef5cc-736c-4622-8fa5-33fb24cead64"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveTargetCursor"",
                    ""type"": ""Button"",
                    ""id"": ""c1328339-d94b-4093-8077-96f7596175f5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bba52b7a-e5b8-4949-8987-bdfb8fcea259"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f92220e-8a7e-4355-8c95-ab0cc40529c6"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TestAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc1e21bf-5ba2-4ae5-9a31-b7c413120916"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""MoveTargetCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_MoveTowerCursor = m_GamePlay.FindAction("MoveTowerCursor", throwIfNotFound: true);
        m_GamePlay_TestAction = m_GamePlay.FindAction("TestAction", throwIfNotFound: true);
        m_GamePlay_MoveTargetCursor = m_GamePlay.FindAction("MoveTargetCursor", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_MoveTowerCursor;
    private readonly InputAction m_GamePlay_TestAction;
    private readonly InputAction m_GamePlay_MoveTargetCursor;
    public struct GamePlayActions
    {
        private @PlayerInput m_Wrapper;
        public GamePlayActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveTowerCursor => m_Wrapper.m_GamePlay_MoveTowerCursor;
        public InputAction @TestAction => m_Wrapper.m_GamePlay_TestAction;
        public InputAction @MoveTargetCursor => m_Wrapper.m_GamePlay_MoveTargetCursor;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @MoveTowerCursor.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTowerCursor;
                @MoveTowerCursor.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTowerCursor;
                @MoveTowerCursor.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTowerCursor;
                @TestAction.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTestAction;
                @TestAction.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTestAction;
                @TestAction.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTestAction;
                @MoveTargetCursor.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTargetCursor;
                @MoveTargetCursor.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTargetCursor;
                @MoveTargetCursor.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTargetCursor;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveTowerCursor.started += instance.OnMoveTowerCursor;
                @MoveTowerCursor.performed += instance.OnMoveTowerCursor;
                @MoveTowerCursor.canceled += instance.OnMoveTowerCursor;
                @TestAction.started += instance.OnTestAction;
                @TestAction.performed += instance.OnTestAction;
                @TestAction.canceled += instance.OnTestAction;
                @MoveTargetCursor.started += instance.OnMoveTargetCursor;
                @MoveTargetCursor.performed += instance.OnMoveTargetCursor;
                @MoveTargetCursor.canceled += instance.OnMoveTargetCursor;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamePadSchemeIndex = -1;
    public InputControlScheme GamePadScheme
    {
        get
        {
            if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
            return asset.controlSchemes[m_GamePadSchemeIndex];
        }
    }
    public interface IGamePlayActions
    {
        void OnMoveTowerCursor(InputAction.CallbackContext context);
        void OnTestAction(InputAction.CallbackContext context);
        void OnMoveTargetCursor(InputAction.CallbackContext context);
    }
}
