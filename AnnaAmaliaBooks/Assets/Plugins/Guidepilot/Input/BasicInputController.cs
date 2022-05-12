#if GUIDEPILOT_CORE_INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BasicInputController : MonoSingleton<BasicInputController>
{
    public static UnityAction<InputEventData> ev_touch_begin;
    public static UnityAction ev_touch_finished;

    public static UnityAction<InputEventData> ev_tap_performed;
    public static UnityAction<InputEventData> ev_tap_started;
    public static UnityAction ev_tap_canceled;

    public static UnityAction<InputEventData> ev_doubletap_performed;
    public static UnityAction<InputEventData> ev_doubletap_started;
    public static UnityAction ev_doubletap_canceled;

    public static UnityAction<InputEventData> ev_drag_performed;
    public static UnityAction<InputEventData> ev_drag_started;
    public static UnityAction ev_drag_canceled;

    public static UnityAction<InputEventData> ev_twofingerdrag_performed;
    public static UnityAction<InputEventData> ev_twofingerdrag_started;
    public static UnityAction ev_twofingerdrag_canceled;

    public static UnityAction<InputEventData> ev_pinch_performed;
    public static UnityAction<InputEventData> ev_pinch_started;
    public static UnityAction ev_pinch_canceled;

    public static UnityAction<InputEventData> ev_twist_performed;
    public static UnityAction ev_twist_started;
    public static UnityAction ev_twist_canceled;

    [BoxGroup("Settings")]
    [SerializeField]
    private bool debug = false;

    [BoxGroup("Settings")]
    [SerializeField]
    private bool dontDestroyOnLoad = false;

    [BoxGroup("Settings")]
    [SerializeField]
    private float dragThreshold = 0;

    [BoxGroup("Settings")]
    [SerializeField]
    private float pinchThreshold = 0.1f;

    [BoxGroup("Settings")]
    [SerializeField]
    private float twistAngleThreshold = 5;

    [BoxGroup("Settings")]
    [SerializeField]
    private float twistFingerDistanceThreshold = 0;

    [BoxGroup("Settings")]
    [SerializeField]
    private float panThreshold = 1f;

    [BoxGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool isDragging = false;

    [BoxGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool isTwoFingerDragging = false;

    [BoxGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool isPinching = false;

    [BoxGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private bool isTwisting = false;

    private UserInput controls;

    private Coroutine dragCoroutine;
    private Coroutine gestureCoroutine;

    private Vector2[] lastFingerPosition = new Vector2[2];
    private float lastTwistAngle;

    private void OnEnable()
    {
        controls.Enable();

        // point and press
        controls.Pointer.PrimaryTouchContact.started += _ => SingleTouchStart();
        controls.Pointer.PrimaryTouchContact.canceled += _ => SingleTouchEnd();

        controls.Pointer.Click.performed += _ => OnPointerPress();
        controls.Pointer.Click.started += _ => FireTapStarted();
        controls.Pointer.Click.canceled += _ => FireTapCanceled();

        controls.Pointer.DoubleClick.performed += _ => OnPointerDoublePress();

        // drag
        controls.Pointer.Press.started += _ => DragStart();
        controls.Pointer.Press.canceled += _ => DragEnd();

        // wheel
        controls.Pointer.Scroll.performed += _ => OnWheel();
        controls.Pointer.Scroll.started += _ => FirePinchStarted();
        controls.Pointer.Scroll.canceled += _ => FirePinchCanceled();

        // multitouch gestures
        controls.MultiTouch.SecondaryTouchContact.started += _ => MultiTouchStart();
        controls.MultiTouch.SecondaryTouchContact.canceled += _ => MultiTouchEnd();
    }

    private void OnDisable()
    {
        controls.Disable();

        // point and press
        controls.Pointer.PrimaryTouchContact.started -= _ => SingleTouchStart();
        controls.Pointer.PrimaryTouchContact.canceled -= _ => SingleTouchEnd();

        controls.Pointer.Click.performed -= _ => OnPointerPress();
        controls.Pointer.Click.started -= _ => FireTapStarted();
        controls.Pointer.Click.canceled -= _ => FireTapCanceled();

        controls.Pointer.DoubleClick.performed -= _ => OnPointerDoublePress();

        // drag
        controls.Pointer.Click.started -= _ => DragStart();
        controls.Pointer.Click.canceled -= _ => DragEnd();

        // wheel
        controls.Pointer.Scroll.performed -= _ => OnWheel();
        controls.Pointer.Scroll.started -= _ => FirePinchStarted();
        controls.Pointer.Scroll.canceled -= _ => FirePinchCanceled();

        // multitouch gestures
        controls.MultiTouch.SecondaryTouchContact.started -= _ => MultiTouchStart();
        controls.MultiTouch.SecondaryTouchContact.canceled -= _ => MultiTouchEnd();
    }

    private void Awake()
    {
        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        Instance = this;

        controls = new UserInput();
    }

    private void Update()
    {
    }

    private void OnPointerPress()
    {
        FireTap(controls.Pointer.Point.ReadValue<Vector2>());
    }

    private void OnPointerDoublePress()
    {
        FireDoubleTap(controls.Pointer.Point.ReadValue<Vector2>());
    }

    private void OnWheel()
    {
        float multiplier = 0.1f;

        FirePinch(controls.Pointer.Scroll.ReadValue<Vector2>().y * multiplier);
    }

    private void DragStart()
    {
        dragCoroutine = StartCoroutine(DragDetection());
    }

    private void DragEnd()
    {
        if (dragCoroutine != null)
        {
            StopCoroutine(dragCoroutine);
        }

        FireDragCanceled();
    }

    private void SingleTouchStart()
    {
        FireTapStarted();
    }

    private void SingleTouchEnd()
    {
        FireFingersUp();
    }

    private void MultiTouchStart()
    {
        DragEnd();

        gestureCoroutine = StartCoroutine(MultiTouchGestureDetection());
    }

    private void MultiTouchEnd()
    {
        StopCoroutine(gestureCoroutine);

        FireTwoFingerDragCanceled();
        FirePinchCanceled();
        FireTwistCanceled();
    }

    IEnumerator DragDetection()
    {
        Vector2 previousPosition = Vector2.zero;
        isDragging = false;

        while (true)
        {
            Vector2 pointerPos = controls.Pointer.Point.ReadValue<Vector2>();
            Vector2 dragDelta = -controls.Pointer.Drag.ReadValue<Vector2>();

            if (Mathf.Abs(dragDelta.magnitude) > dragThreshold)
            {
                FireDragStarted(pointerPos);
                FireDrag(dragDelta, pointerPos);
            }

            yield return null;
        }
    }

    IEnumerator MultiTouchGestureDetection()
    {
        float prevDist = 0;
        float dist = 0;

        isPinching = false;
        isTwisting = false;
        isTwoFingerDragging = false;

        Vector2 primaryPos = controls.MultiTouch.PrimaryFingerPosition.ReadValue<Vector2>();
        Vector2 secondaryPos = controls.MultiTouch.SecondaryFingerPosition.ReadValue<Vector2>();

        Vector2 startVector = secondaryPos - primaryPos;

        lastFingerPosition[0] = primaryPos;
        lastFingerPosition[1] = secondaryPos;
        lastTwistAngle = 0;

        while (true)
        {
            primaryPos = controls.MultiTouch.PrimaryFingerPosition.ReadValue<Vector2>();
            secondaryPos = controls.MultiTouch.SecondaryFingerPosition.ReadValue<Vector2>();

            // simulate second finger for panning, if not used hardware-vise
            // This happens when SecondaryTouchContact was not triggered by a real touch input
            // i.e. by pressing a key or button
            if (secondaryPos.magnitude == 0)
            {
                secondaryPos = lastFingerPosition[0];
                lastTwistAngle = 0;
            }


            dist = Vector2.Distance(primaryPos, secondaryPos);
            float dist_relative = dist / Mathf.Max(Screen.width, Screen.height);

            Vector2 primaryDelta = primaryPos - lastFingerPosition[0];
            Vector2 secondaryDelta = secondaryPos - lastFingerPosition[1];

            Vector2 fingerDelta = secondaryPos - primaryPos;
            float angleOffset = Vector2.Angle(startVector, fingerDelta);

            // pinch fingers
            float dotProduct = Vector2.Dot(primaryDelta, secondaryDelta);
            if (Mathf.Abs(dotProduct) > 0.9f)
            {
                // detect, if fingers move in same or opposite direction
                if (Mathf.Sign(dotProduct) == -1) // opposite direction
                {
                    float deltaDistance = dist - prevDist;

                    if (Mathf.Abs(deltaDistance) > pinchThreshold)
                    {
                        FirePinchStarted();
                        FirePinch(deltaDistance);
                    }
                }
                else if (Mathf.Sign(dotProduct) == 1) // same direction
                {
                    Vector2 deltaDirection = primaryDelta;

                    if (deltaDirection.magnitude > panThreshold)
                    {
                        FireTwoFingerDragStarted(primaryPos);
                        FireTwoFingerDrag(deltaDirection, primaryPos);
                    }
                }
            }


            // twist fingers
            float twistAngleDelta = angleOffset - lastTwistAngle;
            Vector3 twistDirection = Vector3.Cross(startVector, fingerDelta);

            if (angleOffset > twistAngleThreshold && dist_relative >= twistFingerDistanceThreshold)
            {
                if (twistDirection.z > 0)
                {
                    // rotate CCW
                    FireTwistStarted();
                    FireTwist(-twistAngleDelta);
                }
                else if (twistDirection.z < 0)
                {
                    // rotate CW
                    FireTwistStarted();
                    FireTwist(twistAngleDelta);
                }
            }

            prevDist = dist;

            lastFingerPosition[0] = primaryPos;
            lastFingerPosition[1] = secondaryPos;
            lastTwistAngle = angleOffset;

            yield return null;
        }
    }

    private void FireDrag(Vector2 deltaDirection, Vector2 pointerPosition)
    {
        Vector2 deltaVal = deltaDirection / getScreenSize();

        ev_drag_performed?.Invoke(new InputEventData() { deltaPosition = deltaVal, pointerPosition = pointerPosition });
    }

    private void FireDragStarted(Vector2 pointerPosition)
    {
        if (!isDragging)
        {
            isDragging = true;
            ev_drag_started?.Invoke(new InputEventData() { pointerPosition = pointerPosition});

            LogDebugMessage($"{typeof(BasicInputController).Name}: One Finger Drag Started");
        }
    }

    private void FireDragCanceled()
    {
        if (isDragging)
        {
            isDragging = false;
            ev_drag_canceled?.Invoke();

            LogDebugMessage($"{typeof(BasicInputController).Name}: One Finger Drag Canceled");
        }
    }

    private void FireTwoFingerDrag(Vector2 direction, Vector2 pointerPosition)
    {
        Vector2 val = direction / getScreenSize();

        ev_twofingerdrag_performed?.Invoke(new InputEventData() { deltaPosition = val, pointerPosition = pointerPosition });
    }

    private void FireTwoFingerDragStarted(Vector2 pointerPosition)
    {
        if (!isTwoFingerDragging)
        {
            isTwoFingerDragging = true;
            ev_twofingerdrag_started?.Invoke(new InputEventData() { pointerPosition = pointerPosition });

            LogDebugMessage($"{typeof(BasicInputController).Name}: Two Finger Drag Started");
        }
    }

    private void FireTwoFingerDragCanceled()
    {
        if (isTwoFingerDragging)
        {
            isTwoFingerDragging = false;
            ev_twofingerdrag_canceled?.Invoke();

            LogDebugMessage($"{typeof(BasicInputController).Name}: Two Finger Drag Canceled");
        }
    }

    private void FirePinch(float value)
    {
        float val = value / Mathf.Max(Screen.width, Screen.height);

        ev_pinch_performed?.Invoke(new InputEventData() { deltaDistance = val });
    }

    private void FirePinchStarted()
    {
        if (!isPinching)
        {
            isPinching = true;
            ev_pinch_started?.Invoke(new InputEventData() { });

            LogDebugMessage($"{typeof(BasicInputController).Name}: Pinch Started");
        }
    }

    private void FirePinchCanceled()
    {
        if (isPinching)
            isPinching = false;
        ev_pinch_canceled?.Invoke();

        LogDebugMessage($"{typeof(BasicInputController).Name}: Pinch Canceled");
    }

    private void FireTwist(float angle)
    {
        ev_twist_performed?.Invoke(new InputEventData() { deltaDistance = angle });
    }

    private void FireTwistStarted()
    {
        if (!isTwisting)
        {
            isTwisting = true;
            ev_twist_started?.Invoke();
        }
    }

    private void FireTwistCanceled()
    {
        if (isTwisting)
            isTwisting = false;
        ev_twist_canceled?.Invoke();
    }

    private void FireTap(Vector2 pos)
    {
        ev_tap_performed?.Invoke(new InputEventData() { pointerPosition = pos });

        LogDebugMessage($"{typeof(BasicInputController).Name}: One Finger Tap Performed at {pos}");
    }

    private void FireTapStarted()
    {
        Vector2 pos = controls.Pointer.Point.ReadValue<Vector2>();

        ev_tap_started?.Invoke(new InputEventData() { pointerPosition = pos });
        FireFingersDown(pos);

        LogDebugMessage($"{typeof(BasicInputController).Name}: One Finger Tap Started");
    }

    private void FireTapCanceled()
    {
        ev_tap_canceled?.Invoke();

        LogDebugMessage($"{typeof(BasicInputController).Name}: One Finger Tap Canceled");
    }

    private void FireDoubleTap(Vector2 pos)
    {
        ev_doubletap_performed?.Invoke(new InputEventData() { pointerPosition = pos });
        ev_doubletap_started?.Invoke(new InputEventData() { pointerPosition = pos });
        ev_doubletap_canceled?.Invoke();

        LogDebugMessage($"{typeof(BasicInputController).Name}: DoubleTap");
    }

    private void FireFingersDown(Vector2 pos)
    {
        ev_touch_begin?.Invoke(new InputEventData() { pointerPosition = pos });

        LogDebugMessage($"{typeof(BasicInputController).Name}: One Finger Tap Started");
    }

    private void FireFingersUp()
    {
        ev_touch_finished?.Invoke();

        LogDebugMessage($"{typeof(BasicInputController).Name}: TouchFinished");
    }

    public static Vector2Int getScreenSize()
    {
        return new Vector2Int(Screen.width, Screen.height);
    }

    private void LogDebugMessage(string message)
    {
        if (!debug) return;
        
        Debug.Log(message);
    }

    public struct InputEventData
    {
        public Vector2 pointerPosition;
        public Vector2 deltaPosition;
        public float deltaDistance;
    }
}

#endif
