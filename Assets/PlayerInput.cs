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
                    ""type"": ""Button"",
                    ""id"": ""e76c0444-f393-46fb-85f1-395fc048084e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
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
                },
                {
                    ""name"": ""MoveTowerDebug"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a6ae0765-cc03-439b-a515-869d878105b3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NorthButton"",
                    ""type"": ""Button"",
                    ""id"": ""6f066d78-1552-48cf-8ba3-3e73a75168f8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EastButton"",
                    ""type"": ""Button"",
                    ""id"": ""e6491ba1-beb9-430a-a267-154aedcc29c8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SouthButton"",
                    ""type"": ""Button"",
                    ""id"": ""982254e0-43ea-40a8-99cb-319772a0361b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WestButton"",
                    ""type"": ""Button"",
                    ""id"": ""c0a302f4-aa3b-4eac-a994-872907014648"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0cee9e46-4fbb-4e32-80bc-eb23014acfaa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a1f5153f-174f-4768-8734-c67250fc4c7e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6ee5b1f9-3d88-4e27-b321-aef18e2fd65f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""298452ec-31fa-40e8-95fd-da20d8f9829e"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d8916af6-8f40-43db-9f16-56a294b2f6d1"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
                },
                {
                    ""name"": """",
                    ""id"": ""af73cd2b-5964-4b34-855e-2993111ab2e7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveTowerDebug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04948eeb-592e-432a-ba2d-4c5f1310c6a2"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NorthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bfe330d-e5c6-4113-972b-af84a6ef9788"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EastButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""503baedc-089d-42ee-b2ca-6ff4ef50da73"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SouthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c018e32c-245e-4b2d-a68d-89e3bd89a00d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestButton"",
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
        m_GamePlay_MoveTowerDebug = m_GamePlay.FindAction("MoveTowerDebug", throwIfNotFound: true);
        m_GamePlay_NorthButton = m_GamePlay.FindAction("NorthButton", throwIfNotFound: true);
        m_GamePlay_EastButton = m_GamePlay.FindAction("EastButton", throwIfNotFound: true);
        m_GamePlay_SouthButton = m_GamePlay.FindAction("SouthButton", throwIfNotFound: true);
        m_GamePlay_WestButton = m_GamePlay.FindAction("WestButton", throwIfNotFound: true);
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
    private readonly InputAction m_GamePlay_MoveTowerDebug;
    private readonly InputAction m_GamePlay_NorthButton;
    private readonly InputAction m_GamePlay_EastButton;
    private readonly InputAction m_GamePlay_SouthButton;
    private readonly InputAction m_GamePlay_WestButton;
    public struct GamePlayActions
    {
        private @PlayerInput m_Wrapper;
        public GamePlayActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveTowerCursor => m_Wrapper.m_GamePlay_MoveTowerCursor;
        public InputAction @TestAction => m_Wrapper.m_GamePlay_TestAction;
        public InputAction @MoveTargetCursor => m_Wrapper.m_GamePlay_MoveTargetCursor;
        public InputAction @MoveTowerDebug => m_Wrapper.m_GamePlay_MoveTowerDebug;
        public InputAction @NorthButton => m_Wrapper.m_GamePlay_NorthButton;
        public InputAction @EastButton => m_Wrapper.m_GamePlay_EastButton;
        public InputAction @SouthButton => m_Wrapper.m_GamePlay_SouthButton;
        public InputAction @WestButton => m_Wrapper.m_GamePlay_WestButton;
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
                @MoveTowerDebug.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTowerDebug;
                @MoveTowerDebug.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTowerDebug;
                @MoveTowerDebug.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMoveTowerDebug;
                @NorthButton.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnNorthButton;
                @NorthButton.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnNorthButton;
                @NorthButton.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnNorthButton;
                @EastButton.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnEastButton;
                @EastButton.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnEastButton;
                @EastButton.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnEastButton;
                @SouthButton.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSouthButton;
                @SouthButton.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSouthButton;
                @SouthButton.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSouthButton;
                @WestButton.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnWestButton;
                @WestButton.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnWestButton;
                @WestButton.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnWestButton;
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
                @MoveTowerDebug.started += instance.OnMoveTowerDebug;
                @MoveTowerDebug.performed += instance.OnMoveTowerDebug;
                @MoveTowerDebug.canceled += instance.OnMoveTowerDebug;
                @NorthButton.started += instance.OnNorthButton;
                @NorthButton.performed += instance.OnNorthButton;
                @NorthButton.canceled += instance.OnNorthButton;
                @EastButton.started += instance.OnEastButton;
                @EastButton.performed += instance.OnEastButton;
                @EastButton.canceled += instance.OnEastButton;
                @SouthButton.started += instance.OnSouthButton;
                @SouthButton.performed += instance.OnSouthButton;
                @SouthButton.canceled += instance.OnSouthButton;
                @WestButton.started += instance.OnWestButton;
                @WestButton.performed += instance.OnWestButton;
                @WestButton.canceled += instance.OnWestButton;
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
        void OnMoveTowerDebug(InputAction.CallbackContext context);
        void OnNorthButton(InputAction.CallbackContext context);
        void OnEastButton(InputAction.CallbackContext context);
        void OnSouthButton(InputAction.CallbackContext context);
        void OnWestButton(InputAction.CallbackContext context);
    }
}
