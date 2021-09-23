// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/GeneralClasses/PlayerInput.inputactions'

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
                },
                {
                    ""name"": ""MovePlayer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""81a5adcc-4645-4e2c-a627-6f6f43accf22"",
                    ""expectedControlType"": ""Button"",
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
                    ""name"": ""2D Vector"",
                    ""id"": ""1bb4c368-0b90-465f-a46d-57223eb20851"",
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
                    ""id"": ""b066028d-37b2-4afc-bafd-2983fab31fde"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""65731589-6b2a-4707-8c89-52b489feb68d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ac3fd38c-3dad-4443-a8a2-05e693a26782"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveTowerCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""82cf5b12-a900-4a70-92da-8544200fcc2b"",
                    ""path"": ""<Keyboard>/rightArrow"",
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
                    ""id"": ""89da2b92-e580-4f3f-ad48-7f1abb929c94"",
                    ""path"": ""<Keyboard>/w"",
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
                    ""id"": ""6dad54e3-d043-405c-8e91-1f9009a7b8e5"",
                    ""path"": ""<Keyboard>/d"",
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
                    ""id"": ""01527908-623b-4096-ab60-aa8b6cb648d3"",
                    ""path"": ""<Keyboard>/s"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""4c88f879-3f99-4a68-b974-fba65ee0ca95"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f2deb9ce-d482-4bae-a5f4-4ef65d7103f2"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5fe64e48-cba4-49b2-a969-53aa0690ee7f"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ce4f2f33-1622-4a37-9d7b-3755c0b7888c"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a2aaa2e9-b52c-4d60-958d-8241add53365"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""31bd3874-f2d2-4618-a9aa-17ad581b2f37"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""ItemSelection"",
            ""id"": ""e089fb80-151b-4a74-8c57-859ddf373abe"",
            ""actions"": [
                {
                    ""name"": ""MoveItemCursor"",
                    ""type"": ""Button"",
                    ""id"": ""22f15f0a-8bd3-406d-ada6-11c11e9f5c24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TestAction"",
                    ""type"": ""Button"",
                    ""id"": ""0a3d719f-0543-4110-8c8f-4be0fa282d3d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveTargetCursor"",
                    ""type"": ""Button"",
                    ""id"": ""8c35d56d-38dd-43a3-9bbd-c1a72ac6d6bd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NorthButton"",
                    ""type"": ""Button"",
                    ""id"": ""e5031130-57d8-483a-874b-e17a9438c217"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EastButton"",
                    ""type"": ""Button"",
                    ""id"": ""a6183e14-d5db-4ded-b387-85b29ce84b85"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SouthButton"",
                    ""type"": ""Button"",
                    ""id"": ""b48f3962-7daa-4e5f-8c70-1ddf0fbd273c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WestButton"",
                    ""type"": ""Button"",
                    ""id"": ""39952f84-c112-4d60-8476-b6a3eb865656"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MovePlayer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""52e814f7-0bc1-452e-bdf8-fbac7c14c9c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""31b3f313-19e4-4ddb-b6b1-6a92249586b6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b59dac7d-a4b1-4f73-8d86-a878f249d09f"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dff42975-181d-4c40-b6d8-6973b8625413"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a523665b-6a4a-43d4-9326-dfb9ab3e5292"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d77029c1-b900-45a3-9d8d-cfcfc739c695"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""8c64ece9-71b9-4891-bb0e-749090c23489"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""00e3bd26-843a-4f5e-abb0-c0f4193e40a5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dfa5c983-3997-4b5e-bf99-ed7e7f84f19a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""da4f96fc-f917-4115-999c-ee9224e698bd"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5c8da361-5091-47ff-a9f4-a2fdbc823cc9"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard;GamePad"",
                    ""action"": ""MoveItemCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c20f67ee-a4a6-455e-b045-407e5c44981d"",
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
                    ""id"": ""dd211d9d-610b-408b-8daa-a5df834b7baf"",
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
                    ""id"": ""c0656a7a-0220-44d7-abeb-569a6d357a1f"",
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
                    ""id"": ""73dbdeb8-9024-426e-8905-5e8eb809df1b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NorthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37b08cce-9512-4cd8-8b1f-cc4a133e46ef"",
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
                    ""id"": ""95b9feac-f0f4-4446-b5c9-f8479fd75bab"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EastButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86ff2219-1902-436f-8a05-20bfd6514b15"",
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
                    ""id"": ""051d29ec-eda3-4809-9418-7417a7fb7a06"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SouthButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30d2a62a-e2ca-4dbf-962a-a97a8e3e2b13"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a3b0ad9-6544-4e17-8383-ec1d1e5c7a95"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""4a4ed5a2-8607-4963-aac2-b8052f108850"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3604cdb2-ea93-4c85-ae01-9212bedf315c"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ad2ecbed-6a72-4679-92f5-6a6d8eb48a72"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5e44515f-ac8a-4fe2-bb58-5634157c3753"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""05baafe6-bed3-4cdd-a856-ac75d37645ef"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad;Keyboard"",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""TestButtons"",
            ""id"": ""f7397da4-4cb4-4d0f-a776-d16bef6f8071"",
            ""actions"": [
                {
                    ""name"": ""T"",
                    ""type"": ""Button"",
                    ""id"": ""5e80d883-3bfb-4c36-a464-a18b6065e087"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Y"",
                    ""type"": ""Button"",
                    ""id"": ""fb1d43cd-6b4b-46c8-994f-8a33eacb47de"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""U"",
                    ""type"": ""Button"",
                    ""id"": ""18ab84fa-0593-438b-bbce-39cb13885a58"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""J"",
                    ""type"": ""Button"",
                    ""id"": ""7262049a-2884-43fa-b0e7-d1c7138a6583"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f3115d40-fba4-43e2-b28b-f7fcca7cb011"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""T"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85ff7aac-4834-4d78-8907-b06fab0ebc45"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90acd05d-8b24-4a7f-93f8-199b5f39581e"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""U"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""894131d3-d2cc-42a0-9aea-7ca339f08121"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""J"",
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
        m_GamePlay_NorthButton = m_GamePlay.FindAction("NorthButton", throwIfNotFound: true);
        m_GamePlay_EastButton = m_GamePlay.FindAction("EastButton", throwIfNotFound: true);
        m_GamePlay_SouthButton = m_GamePlay.FindAction("SouthButton", throwIfNotFound: true);
        m_GamePlay_WestButton = m_GamePlay.FindAction("WestButton", throwIfNotFound: true);
        m_GamePlay_MovePlayer = m_GamePlay.FindAction("MovePlayer", throwIfNotFound: true);
        // ItemSelection
        m_ItemSelection = asset.FindActionMap("ItemSelection", throwIfNotFound: true);
        m_ItemSelection_MoveItemCursor = m_ItemSelection.FindAction("MoveItemCursor", throwIfNotFound: true);
        m_ItemSelection_TestAction = m_ItemSelection.FindAction("TestAction", throwIfNotFound: true);
        m_ItemSelection_MoveTargetCursor = m_ItemSelection.FindAction("MoveTargetCursor", throwIfNotFound: true);
        m_ItemSelection_NorthButton = m_ItemSelection.FindAction("NorthButton", throwIfNotFound: true);
        m_ItemSelection_EastButton = m_ItemSelection.FindAction("EastButton", throwIfNotFound: true);
        m_ItemSelection_SouthButton = m_ItemSelection.FindAction("SouthButton", throwIfNotFound: true);
        m_ItemSelection_WestButton = m_ItemSelection.FindAction("WestButton", throwIfNotFound: true);
        m_ItemSelection_MovePlayer = m_ItemSelection.FindAction("MovePlayer", throwIfNotFound: true);
        // TestButtons
        m_TestButtons = asset.FindActionMap("TestButtons", throwIfNotFound: true);
        m_TestButtons_T = m_TestButtons.FindAction("T", throwIfNotFound: true);
        m_TestButtons_Y = m_TestButtons.FindAction("Y", throwIfNotFound: true);
        m_TestButtons_U = m_TestButtons.FindAction("U", throwIfNotFound: true);
        m_TestButtons_J = m_TestButtons.FindAction("J", throwIfNotFound: true);
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
    private readonly InputAction m_GamePlay_NorthButton;
    private readonly InputAction m_GamePlay_EastButton;
    private readonly InputAction m_GamePlay_SouthButton;
    private readonly InputAction m_GamePlay_WestButton;
    private readonly InputAction m_GamePlay_MovePlayer;
    public struct GamePlayActions
    {
        private @PlayerInput m_Wrapper;
        public GamePlayActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveTowerCursor => m_Wrapper.m_GamePlay_MoveTowerCursor;
        public InputAction @TestAction => m_Wrapper.m_GamePlay_TestAction;
        public InputAction @MoveTargetCursor => m_Wrapper.m_GamePlay_MoveTargetCursor;
        public InputAction @NorthButton => m_Wrapper.m_GamePlay_NorthButton;
        public InputAction @EastButton => m_Wrapper.m_GamePlay_EastButton;
        public InputAction @SouthButton => m_Wrapper.m_GamePlay_SouthButton;
        public InputAction @WestButton => m_Wrapper.m_GamePlay_WestButton;
        public InputAction @MovePlayer => m_Wrapper.m_GamePlay_MovePlayer;
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
                @MovePlayer.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMovePlayer;
                @MovePlayer.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMovePlayer;
                @MovePlayer.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMovePlayer;
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
                @MovePlayer.started += instance.OnMovePlayer;
                @MovePlayer.performed += instance.OnMovePlayer;
                @MovePlayer.canceled += instance.OnMovePlayer;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);

    // ItemSelection
    private readonly InputActionMap m_ItemSelection;
    private IItemSelectionActions m_ItemSelectionActionsCallbackInterface;
    private readonly InputAction m_ItemSelection_MoveItemCursor;
    private readonly InputAction m_ItemSelection_TestAction;
    private readonly InputAction m_ItemSelection_MoveTargetCursor;
    private readonly InputAction m_ItemSelection_NorthButton;
    private readonly InputAction m_ItemSelection_EastButton;
    private readonly InputAction m_ItemSelection_SouthButton;
    private readonly InputAction m_ItemSelection_WestButton;
    private readonly InputAction m_ItemSelection_MovePlayer;
    public struct ItemSelectionActions
    {
        private @PlayerInput m_Wrapper;
        public ItemSelectionActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveItemCursor => m_Wrapper.m_ItemSelection_MoveItemCursor;
        public InputAction @TestAction => m_Wrapper.m_ItemSelection_TestAction;
        public InputAction @MoveTargetCursor => m_Wrapper.m_ItemSelection_MoveTargetCursor;
        public InputAction @NorthButton => m_Wrapper.m_ItemSelection_NorthButton;
        public InputAction @EastButton => m_Wrapper.m_ItemSelection_EastButton;
        public InputAction @SouthButton => m_Wrapper.m_ItemSelection_SouthButton;
        public InputAction @WestButton => m_Wrapper.m_ItemSelection_WestButton;
        public InputAction @MovePlayer => m_Wrapper.m_ItemSelection_MovePlayer;
        public InputActionMap Get() { return m_Wrapper.m_ItemSelection; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ItemSelectionActions set) { return set.Get(); }
        public void SetCallbacks(IItemSelectionActions instance)
        {
            if (m_Wrapper.m_ItemSelectionActionsCallbackInterface != null)
            {
                @MoveItemCursor.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMoveItemCursor;
                @MoveItemCursor.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMoveItemCursor;
                @MoveItemCursor.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMoveItemCursor;
                @TestAction.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnTestAction;
                @TestAction.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnTestAction;
                @TestAction.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnTestAction;
                @MoveTargetCursor.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMoveTargetCursor;
                @MoveTargetCursor.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMoveTargetCursor;
                @MoveTargetCursor.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMoveTargetCursor;
                @NorthButton.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnNorthButton;
                @NorthButton.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnNorthButton;
                @NorthButton.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnNorthButton;
                @EastButton.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnEastButton;
                @EastButton.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnEastButton;
                @EastButton.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnEastButton;
                @SouthButton.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnSouthButton;
                @SouthButton.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnSouthButton;
                @SouthButton.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnSouthButton;
                @WestButton.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnWestButton;
                @WestButton.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnWestButton;
                @WestButton.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnWestButton;
                @MovePlayer.started -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMovePlayer;
                @MovePlayer.performed -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMovePlayer;
                @MovePlayer.canceled -= m_Wrapper.m_ItemSelectionActionsCallbackInterface.OnMovePlayer;
            }
            m_Wrapper.m_ItemSelectionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveItemCursor.started += instance.OnMoveItemCursor;
                @MoveItemCursor.performed += instance.OnMoveItemCursor;
                @MoveItemCursor.canceled += instance.OnMoveItemCursor;
                @TestAction.started += instance.OnTestAction;
                @TestAction.performed += instance.OnTestAction;
                @TestAction.canceled += instance.OnTestAction;
                @MoveTargetCursor.started += instance.OnMoveTargetCursor;
                @MoveTargetCursor.performed += instance.OnMoveTargetCursor;
                @MoveTargetCursor.canceled += instance.OnMoveTargetCursor;
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
                @MovePlayer.started += instance.OnMovePlayer;
                @MovePlayer.performed += instance.OnMovePlayer;
                @MovePlayer.canceled += instance.OnMovePlayer;
            }
        }
    }
    public ItemSelectionActions @ItemSelection => new ItemSelectionActions(this);

    // TestButtons
    private readonly InputActionMap m_TestButtons;
    private ITestButtonsActions m_TestButtonsActionsCallbackInterface;
    private readonly InputAction m_TestButtons_T;
    private readonly InputAction m_TestButtons_Y;
    private readonly InputAction m_TestButtons_U;
    private readonly InputAction m_TestButtons_J;
    public struct TestButtonsActions
    {
        private @PlayerInput m_Wrapper;
        public TestButtonsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @T => m_Wrapper.m_TestButtons_T;
        public InputAction @Y => m_Wrapper.m_TestButtons_Y;
        public InputAction @U => m_Wrapper.m_TestButtons_U;
        public InputAction @J => m_Wrapper.m_TestButtons_J;
        public InputActionMap Get() { return m_Wrapper.m_TestButtons; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestButtonsActions set) { return set.Get(); }
        public void SetCallbacks(ITestButtonsActions instance)
        {
            if (m_Wrapper.m_TestButtonsActionsCallbackInterface != null)
            {
                @T.started -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnT;
                @T.performed -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnT;
                @T.canceled -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnT;
                @Y.started -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnY;
                @Y.performed -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnY;
                @Y.canceled -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnY;
                @U.started -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnU;
                @U.performed -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnU;
                @U.canceled -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnU;
                @J.started -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnJ;
                @J.performed -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnJ;
                @J.canceled -= m_Wrapper.m_TestButtonsActionsCallbackInterface.OnJ;
            }
            m_Wrapper.m_TestButtonsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @T.started += instance.OnT;
                @T.performed += instance.OnT;
                @T.canceled += instance.OnT;
                @Y.started += instance.OnY;
                @Y.performed += instance.OnY;
                @Y.canceled += instance.OnY;
                @U.started += instance.OnU;
                @U.performed += instance.OnU;
                @U.canceled += instance.OnU;
                @J.started += instance.OnJ;
                @J.performed += instance.OnJ;
                @J.canceled += instance.OnJ;
            }
        }
    }
    public TestButtonsActions @TestButtons => new TestButtonsActions(this);
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
        void OnNorthButton(InputAction.CallbackContext context);
        void OnEastButton(InputAction.CallbackContext context);
        void OnSouthButton(InputAction.CallbackContext context);
        void OnWestButton(InputAction.CallbackContext context);
        void OnMovePlayer(InputAction.CallbackContext context);
    }
    public interface IItemSelectionActions
    {
        void OnMoveItemCursor(InputAction.CallbackContext context);
        void OnTestAction(InputAction.CallbackContext context);
        void OnMoveTargetCursor(InputAction.CallbackContext context);
        void OnNorthButton(InputAction.CallbackContext context);
        void OnEastButton(InputAction.CallbackContext context);
        void OnSouthButton(InputAction.CallbackContext context);
        void OnWestButton(InputAction.CallbackContext context);
        void OnMovePlayer(InputAction.CallbackContext context);
    }
    public interface ITestButtonsActions
    {
        void OnT(InputAction.CallbackContext context);
        void OnY(InputAction.CallbackContext context);
        void OnU(InputAction.CallbackContext context);
        void OnJ(InputAction.CallbackContext context);
    }
}
