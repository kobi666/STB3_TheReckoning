// GENERATED AUTOMATICALLY FROM 'Assets/Garbage/TestingHold.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TestingHold : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestingHold()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestingHold"",
    ""maps"": [
        {
            ""name"": ""Test1"",
            ""id"": ""8576050f-e31d-428a-80c7-76e7203e028d"",
            ""actions"": [
                {
                    ""name"": ""HoldAction"",
                    ""type"": ""Button"",
                    ""id"": ""59e45da6-fa7b-49c2-a9e3-4feb0b85bf64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=1.5)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0ebdc7c3-032e-4ff8-9911-37aed033ef18"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Test1
        m_Test1 = asset.FindActionMap("Test1", throwIfNotFound: true);
        m_Test1_HoldAction = m_Test1.FindAction("HoldAction", throwIfNotFound: true);
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

    // Test1
    private readonly InputActionMap m_Test1;
    private ITest1Actions m_Test1ActionsCallbackInterface;
    private readonly InputAction m_Test1_HoldAction;
    public struct Test1Actions
    {
        private @TestingHold m_Wrapper;
        public Test1Actions(@TestingHold wrapper) { m_Wrapper = wrapper; }
        public InputAction @HoldAction => m_Wrapper.m_Test1_HoldAction;
        public InputActionMap Get() { return m_Wrapper.m_Test1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Test1Actions set) { return set.Get(); }
        public void SetCallbacks(ITest1Actions instance)
        {
            if (m_Wrapper.m_Test1ActionsCallbackInterface != null)
            {
                @HoldAction.started -= m_Wrapper.m_Test1ActionsCallbackInterface.OnHoldAction;
                @HoldAction.performed -= m_Wrapper.m_Test1ActionsCallbackInterface.OnHoldAction;
                @HoldAction.canceled -= m_Wrapper.m_Test1ActionsCallbackInterface.OnHoldAction;
            }
            m_Wrapper.m_Test1ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HoldAction.started += instance.OnHoldAction;
                @HoldAction.performed += instance.OnHoldAction;
                @HoldAction.canceled += instance.OnHoldAction;
            }
        }
    }
    public Test1Actions @Test1 => new Test1Actions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface ITest1Actions
    {
        void OnHoldAction(InputAction.CallbackContext context);
    }
}
