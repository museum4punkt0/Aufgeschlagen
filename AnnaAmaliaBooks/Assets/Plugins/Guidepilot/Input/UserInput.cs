// GENERATED AUTOMATICALLY FROM 'Assets/Plugins/Guidepilot/Input/UserInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @UserInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @UserInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UserInput"",
    ""maps"": [
        {
            ""name"": ""Pointer"",
            ""id"": ""72546a34-5b0e-4e5b-8343-b51f528b7057"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""8cc9b415-9a17-4f93-8993-eae8021abf15"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DoubleClick"",
                    ""type"": ""Button"",
                    ""id"": ""95fa1293-72d2-4104-a5a8-f4280d49169e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Press"",
                    ""type"": ""Button"",
                    ""id"": ""b01ad838-fa25-453d-a078-b7818abd1e16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""Value"",
                    ""id"": ""2149c443-0e3f-4877-8036-e6e4ca898e67"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drag"",
                    ""type"": ""Value"",
                    ""id"": ""b59a54ae-8e49-4477-97be-3861f6c57728"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""e424fb33-6589-4e0a-a569-3e3d2eaa5db0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryTouchContact"",
                    ""type"": ""Button"",
                    ""id"": ""cb8d2720-e316-44ef-8953-e880b21ee092"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ef43af0d-d791-482a-8529-1794d3090623"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a146cf83-2984-42f1-9a77-3a8b12cf02ea"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e830fd45-7158-4fc4-ac7e-364d5e3757af"",
                    ""path"": ""<Touchscreen>/touch0/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05694899-9e9d-4d23-afc5-8be96d7049d9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9f1fa1e-fa09-43c1-b893-debab651aab5"",
                    ""path"": ""<Touchscreen>/touch0/tap"",
                    ""interactions"": ""MultiTap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DoubleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b73a594f-e844-4b96-b9b5-92762fbc65a4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""MultiTap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DoubleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8b528f9-5ea9-4908-98ca-7b0be7452404"",
                    ""path"": ""<Touchscreen>/touch0/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dda5efba-8a7f-4ab0-9e2e-0dca976c5bfb"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a829fd29-f029-4c1f-996b-3bdf36ef1aad"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd8d217b-02b0-4695-b587-deed7f5a01e8"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f1adb85-1468-4553-a42c-8f7d47b919bd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ecb9b1b-af1e-4dfb-aeb4-81a0f3b8dc1c"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""480f72cd-4cd5-44d4-a2a7-0f46deb79f99"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MultiTouch"",
            ""id"": ""6911bea5-8c88-47f5-af76-5bc9098ff060"",
            ""actions"": [
                {
                    ""name"": ""PrimaryFingerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""b6dc535b-f574-43cd-a641-e44361dd974f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryFingerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""f40d8cdf-cda9-4038-bda6-0f50189c3c05"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryTouchContact"",
                    ""type"": ""Button"",
                    ""id"": ""283f9e27-d75f-4678-8242-ec6910108ee3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d8d7f52d-0593-4944-9752-d82e3501cb01"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8850bde-832c-46b4-bef2-a2868eb793b0"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d777e93-289f-4e0a-8868-beb9ab11120c"",
                    ""path"": ""<Touchscreen>/touch1/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryFingerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e124b1f-dc7e-4c4f-b4eb-5b3e50c742ee"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be9fa0d8-23aa-4e35-acb8-a5675e46ab4f"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryTouchContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Pointer
        m_Pointer = asset.FindActionMap("Pointer", throwIfNotFound: true);
        m_Pointer_Click = m_Pointer.FindAction("Click", throwIfNotFound: true);
        m_Pointer_DoubleClick = m_Pointer.FindAction("DoubleClick", throwIfNotFound: true);
        m_Pointer_Press = m_Pointer.FindAction("Press", throwIfNotFound: true);
        m_Pointer_Point = m_Pointer.FindAction("Point", throwIfNotFound: true);
        m_Pointer_Drag = m_Pointer.FindAction("Drag", throwIfNotFound: true);
        m_Pointer_Scroll = m_Pointer.FindAction("Scroll", throwIfNotFound: true);
        m_Pointer_PrimaryTouchContact = m_Pointer.FindAction("PrimaryTouchContact", throwIfNotFound: true);
        // MultiTouch
        m_MultiTouch = asset.FindActionMap("MultiTouch", throwIfNotFound: true);
        m_MultiTouch_PrimaryFingerPosition = m_MultiTouch.FindAction("PrimaryFingerPosition", throwIfNotFound: true);
        m_MultiTouch_SecondaryFingerPosition = m_MultiTouch.FindAction("SecondaryFingerPosition", throwIfNotFound: true);
        m_MultiTouch_SecondaryTouchContact = m_MultiTouch.FindAction("SecondaryTouchContact", throwIfNotFound: true);
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

    // Pointer
    private readonly InputActionMap m_Pointer;
    private IPointerActions m_PointerActionsCallbackInterface;
    private readonly InputAction m_Pointer_Click;
    private readonly InputAction m_Pointer_DoubleClick;
    private readonly InputAction m_Pointer_Press;
    private readonly InputAction m_Pointer_Point;
    private readonly InputAction m_Pointer_Drag;
    private readonly InputAction m_Pointer_Scroll;
    private readonly InputAction m_Pointer_PrimaryTouchContact;
    public struct PointerActions
    {
        private @UserInput m_Wrapper;
        public PointerActions(@UserInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_Pointer_Click;
        public InputAction @DoubleClick => m_Wrapper.m_Pointer_DoubleClick;
        public InputAction @Press => m_Wrapper.m_Pointer_Press;
        public InputAction @Point => m_Wrapper.m_Pointer_Point;
        public InputAction @Drag => m_Wrapper.m_Pointer_Drag;
        public InputAction @Scroll => m_Wrapper.m_Pointer_Scroll;
        public InputAction @PrimaryTouchContact => m_Wrapper.m_Pointer_PrimaryTouchContact;
        public InputActionMap Get() { return m_Wrapper.m_Pointer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PointerActions set) { return set.Get(); }
        public void SetCallbacks(IPointerActions instance)
        {
            if (m_Wrapper.m_PointerActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnClick;
                @DoubleClick.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnDoubleClick;
                @DoubleClick.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnDoubleClick;
                @DoubleClick.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnDoubleClick;
                @Press.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnPress;
                @Press.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnPress;
                @Press.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnPress;
                @Point.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnPoint;
                @Drag.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnDrag;
                @Drag.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnDrag;
                @Drag.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnDrag;
                @Scroll.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnScroll;
                @PrimaryTouchContact.started -= m_Wrapper.m_PointerActionsCallbackInterface.OnPrimaryTouchContact;
                @PrimaryTouchContact.performed -= m_Wrapper.m_PointerActionsCallbackInterface.OnPrimaryTouchContact;
                @PrimaryTouchContact.canceled -= m_Wrapper.m_PointerActionsCallbackInterface.OnPrimaryTouchContact;
            }
            m_Wrapper.m_PointerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @DoubleClick.started += instance.OnDoubleClick;
                @DoubleClick.performed += instance.OnDoubleClick;
                @DoubleClick.canceled += instance.OnDoubleClick;
                @Press.started += instance.OnPress;
                @Press.performed += instance.OnPress;
                @Press.canceled += instance.OnPress;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Drag.started += instance.OnDrag;
                @Drag.performed += instance.OnDrag;
                @Drag.canceled += instance.OnDrag;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @PrimaryTouchContact.started += instance.OnPrimaryTouchContact;
                @PrimaryTouchContact.performed += instance.OnPrimaryTouchContact;
                @PrimaryTouchContact.canceled += instance.OnPrimaryTouchContact;
            }
        }
    }
    public PointerActions @Pointer => new PointerActions(this);

    // MultiTouch
    private readonly InputActionMap m_MultiTouch;
    private IMultiTouchActions m_MultiTouchActionsCallbackInterface;
    private readonly InputAction m_MultiTouch_PrimaryFingerPosition;
    private readonly InputAction m_MultiTouch_SecondaryFingerPosition;
    private readonly InputAction m_MultiTouch_SecondaryTouchContact;
    public struct MultiTouchActions
    {
        private @UserInput m_Wrapper;
        public MultiTouchActions(@UserInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryFingerPosition => m_Wrapper.m_MultiTouch_PrimaryFingerPosition;
        public InputAction @SecondaryFingerPosition => m_Wrapper.m_MultiTouch_SecondaryFingerPosition;
        public InputAction @SecondaryTouchContact => m_Wrapper.m_MultiTouch_SecondaryTouchContact;
        public InputActionMap Get() { return m_Wrapper.m_MultiTouch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MultiTouchActions set) { return set.Get(); }
        public void SetCallbacks(IMultiTouchActions instance)
        {
            if (m_Wrapper.m_MultiTouchActionsCallbackInterface != null)
            {
                @PrimaryFingerPosition.started -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.performed -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.canceled -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnPrimaryFingerPosition;
                @SecondaryFingerPosition.started -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.performed -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.canceled -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnSecondaryFingerPosition;
                @SecondaryTouchContact.started -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled -= m_Wrapper.m_MultiTouchActionsCallbackInterface.OnSecondaryTouchContact;
            }
            m_Wrapper.m_MultiTouchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PrimaryFingerPosition.started += instance.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.performed += instance.OnPrimaryFingerPosition;
                @PrimaryFingerPosition.canceled += instance.OnPrimaryFingerPosition;
                @SecondaryFingerPosition.started += instance.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.performed += instance.OnSecondaryFingerPosition;
                @SecondaryFingerPosition.canceled += instance.OnSecondaryFingerPosition;
                @SecondaryTouchContact.started += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.performed += instance.OnSecondaryTouchContact;
                @SecondaryTouchContact.canceled += instance.OnSecondaryTouchContact;
            }
        }
    }
    public MultiTouchActions @MultiTouch => new MultiTouchActions(this);
    public interface IPointerActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnDoubleClick(InputAction.CallbackContext context);
        void OnPress(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnDrag(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnPrimaryTouchContact(InputAction.CallbackContext context);
    }
    public interface IMultiTouchActions
    {
        void OnPrimaryFingerPosition(InputAction.CallbackContext context);
        void OnSecondaryFingerPosition(InputAction.CallbackContext context);
        void OnSecondaryTouchContact(InputAction.CallbackContext context);
    }
}
