#if GUIDEPILOT_CORE_AR

using com.guidepilot.guidepilotcore;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ARViewerContentController : MonoBehaviour
{
    [BoxGroup("General")]
    [InlineEditor]
    [SerializeField]
    private ARViewerConfiguration configuration;

    [BoxGroup("General")]
    [InlineEditor]
    [SerializeField]
    private Material transparentMaterial;

    [BoxGroup("Settings")]
    public bool initializeOnStart = true;

    [BoxGroup("Settings")]
    public ViewerCameraControllerTarget.CameraTransitionSettings cameraTransitionSettings;

    [BoxGroup("References")]
    [SerializeField]
    private ViewerCameraControllerTarget currentTarget;

    [BoxGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private ARViewerSceneController sceneController;

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public bool initialized { get; private set; }

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public bool inputEventsEnabled { get; private set; }

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    private bool isLoaded = false;

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    private bool isPlaced = false;

    private List<Material> defaultMaterials = new List<Material>();

    private Vector3 initialScale = Vector3.one;
    private Vector3 dragOffset;

    private Vector3 cameraTargetScreenPosition;


    private void OnEnable() => EnableInputEvents();


    private void OnDisable() => DisableInputEvents();

    private void Awake()
    {
        ARViewerSceneController.OnInitializationCompleteEvent += OnARViewerSceneControllerInitializationComplete;

        sceneController = FindObjectOfType<ARViewerSceneController>();
        this.enabled = (sceneController != null);
    }

    private void OnDestroy()
    {
        ARViewerSceneController.OnInitializationCompleteEvent -= OnARViewerSceneControllerInitializationComplete;
    }

    void Start()
    {
        if (initializeOnStart)
            Initialize(configuration);
    }

    private void Update()
    {
        if (isLoaded && !isPlaced)
        {
            SpawnContentOnSurface();
        }
    }

    public void Initialize(ARViewerConfiguration configuration)
    {
        if (configuration == null)
        {
            Debug.LogWarning($"{typeof(ARViewerContentController).Name}: Can't initialize because configuration is null");
            return;
        }

        this.enabled = true;
        this.configuration = configuration;

        EnableInputEvents();

        if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.SurfaceTracking)
        {
            ViewerCameraControllerTarget newTarget = FindObjectOfType<ViewerCameraControllerTarget>();
            if (newTarget != null)
            {
                newTarget.gameObject.SetActive(false);
                newTarget.transform.SetParent(sceneController.getOriginTransform());
                SetCameraTarget(newTarget);
                SetTargetTransparent(true);

                isLoaded = true;
            }
        }

        initialized = true;
    }

    private void SpawnContentOnSurface()
    {
        if (isLoaded && !isPlaced)
        {
            Pose hitPose;

            if (sceneController.RaycastARPlane(sceneController.GetARcamera().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0)), out hitPose))
            {
                if (currentTarget != null)
                {
                    currentTarget.transform.position = hitPose.position;
                    currentTarget.gameObject.SetActive(true);
                }
            }
        }
    }

    private void PlaceContentOnSurface(BasicInputController.InputEventData ev)
    {
        if (isLoaded && !isPlaced)
        {
            Pose hitPose;

            if (sceneController.RaycastARPlane(sceneController.GetARcamera().ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0)), out hitPose))
            {
                if (currentTarget != null)
                {
                    // disable plane decection, hide plane meshes
                    sceneController.TogglePlaneDetection();

                    // place and show content
                    currentTarget.transform.SetParent(sceneController.getOriginTransform());
                    currentTarget.transform.position = hitPose.position;
                    currentTarget.gameObject.SetActive(true);

                    // set opaque
                    SetTargetTransparent(false);

                    // place shadow receiver plane
                    if (configuration.shadowReceiverPrefab != null)
                    {
                        GameObject receiverGO = Instantiate(configuration.shadowReceiverPrefab);
                        receiverGO.transform.position = currentTarget.transform.position;
                        receiverGO.transform.SetParent(currentTarget.transform);
                    }

                    isPlaced = true;
                }
            }
        }
    }

    private void ShowWorldUI(BasicInputController.InputEventData ev)
    {
        if (isPlaced)
        {
            if (sceneController != null)
            {
                sceneController.SetPlanesVisibility(true);
            }
        }
    }

    private void HideWorldUI()
    {
        if (isPlaced)
        {
            if (sceneController != null)
            {
                sceneController.SetPlanesVisibility(false);
            }
        }
    }

    private void GetTargetDefaultMaterials()
    {
        defaultMaterials.Clear();

        MeshRenderer[] rendererList = currentTarget.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in rendererList)
        {
            defaultMaterials.Add(r.material);
        }
    }

    private void SetTargetTransparent(bool isTransparent)
    {
        if (defaultMaterials.Count == 0)
            GetTargetDefaultMaterials();
        
        float alpha = isTransparent ? 0.6f : 1f; ;

        MeshRenderer[] rendererList = currentTarget.gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < rendererList.Length; i++)
        {
            if (isTransparent)
            {
                Material originalMat = defaultMaterials[i];
                Material transparentMat = new Material(transparentMaterial);
                //transparentMat.CopyPropertiesFromMaterial(originalMat);
                rendererList[i].material = transparentMat;
            } else
            {
                rendererList[i].material = defaultMaterials[i];
            }

            Color c = rendererList[i].material.color;
            c.a = alpha;
            rendererList[i].material.color = c;
        }
    }

    #region PAN_CONTROL
    public void PanInit(BasicInputController.InputEventData ev)
    {
        if (!configuration.enablePan || currentTarget == null) return;

        if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.SurfaceTracking)
        {
            Pose hitPose;

            if (sceneController.RaycastARPlane(ev.pointerPosition, out hitPose))
            {
                dragOffset = currentTarget.transform.position - hitPose.position;
            }
        }
        else if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.ImageTracking)
        {
            //TODO: Not working currently
            //cameraTargetScreenPosition = Camera.main.WorldToScreenPoint(currentTarget.transform.position);
            //
            //Vector3 currentScreenPoint = new Vector3(ev.pointerPosition.x, ev.pointerPosition.y, cameraTargetScreenPosition.z);
            //
            //dragOffset = cameraTargetScreenPosition - currentScreenPoint;
            //
            //Debug.Log($"Init: {cameraTargetScreenPosition} / {currentScreenPoint} / {dragOffset}");
        }

    }

    public void Pan(BasicInputController.InputEventData ev)
    {
        if (!configuration.enablePan || currentTarget == null) return;

        if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.SurfaceTracking)
        {
            Pose hitPose;

            if (sceneController.RaycastARPlane(ev.pointerPosition, out hitPose))
            {
                currentTarget.transform.position = hitPose.position + dragOffset;
            }
        }
        else if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.ImageTracking)
        {
            //TODO: Not working currently
            //Vector3 currentScreenPoint = new Vector3(ev.pointerPosition.x, ev.pointerPosition.y, cameraTargetScreenPosition.z);
            //Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint(currentScreenPoint + dragOffset);
            //
            //Vector3 movementVector = -currentWorldPoint;
            //
            //currentTarget.transform.position += movementVector;

            //Debug.Log(currentScreenPoint);
        }

        //IsTargetOutOfView();
    }
    #endregion

    #region ROTATION_CONTROL
    public void Rotate(BasicInputController.InputEventData ev)
    {
        if (!configuration.enableRotation || currentTarget == null) return;

        if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.SurfaceTracking)
        {
            Vector2 rotationVector = new Vector2(0, ev.deltaDistance);
            Vector3 lookRotation = configuration.rotationSpeed * rotationVector * Time.unscaledDeltaTime;
            currentTarget.transform.Rotate(lookRotation);
        }
        else if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.ImageTracking)
        {
            if (currentTarget.usePhysics)
            {
                currentTarget.rigidbody.AddTorque(Camera.main.transform.up * ev.deltaPosition.x * configuration.rotationSpeed, currentTarget.forceMode);
                currentTarget.rigidbody.AddTorque(Camera.main.transform.right * -ev.deltaPosition.y * configuration.rotationSpeed, currentTarget.forceMode);
            }
            else
            {
                currentTarget.transform.RotateAround(currentTarget.transform.position, Camera.main.transform.up, ev.deltaPosition.x * configuration.rotationSpeed);
                currentTarget.transform.RotateAround(currentTarget.transform.position, Camera.main.transform.right, -ev.deltaPosition.y * configuration.rotationSpeed);
                currentTarget.transform.eulerAngles += new Vector3(ev.deltaPosition.y, ev.deltaPosition.x) * configuration.rotationSpeed;
            }
        }
    }
    #endregion

    #region SCALE_CONTROL
    public void Scale(BasicInputController.InputEventData ev)
    {
        if (!configuration.enableZoomScale || currentTarget == null) return;

        if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.SurfaceTracking)
        {
            Vector3 minScale = initialScale * configuration.minScaleFactor;
            Vector3 maxScale = initialScale * configuration.maxScaleFactor;

            Vector3 newScale = currentTarget.transform.localScale + Vector3.one * ev.deltaDistance * configuration.zoomScaleSpeed * Time.unscaledDeltaTime;

            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
                Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
                Mathf.Clamp(newScale.z, minScale.z, maxScale.z)
                );

            currentTarget.transform.localScale = newScale;
        }
        else if (configuration.trackingMode == ARViewerConfiguration.TrackingMode.ImageTracking)
        {
            // TODO
        }
    }
    #endregion

    [Button(ButtonSizes.Large, ButtonStyle.CompactBox)]
    public void ResetPositionAndRotation(ViewerCameraControllerTarget.CameraTransitionSettings cameraTransitionSettings = null)
    {
        if (currentTarget == null) return;

        if (cameraTransitionSettings == null)
            cameraTransitionSettings = this.cameraTransitionSettings;

        CalculateInitialPositionAndRotation(currentTarget);
    }

    public void SetCameraTarget(ViewerCameraControllerTarget target)
    {
        if (currentTarget == target || target == null) return;

        currentTarget = target;

        ResetPositionAndRotation(currentTarget.cameraTransitionSettings);

        Debug.Log($"{typeof(ViewerCameraController).Name}: Set camera target to: {target.name}");
    }

    private void CalculateInitialPositionAndRotation(ViewerCameraControllerTarget target)
    {
        Bounds bounds = target.rendererBounds;

        //Debug.Log("Center: " + bounds.center + "\n Extents: " + bounds.extents + "\n Size: " + bounds.size + "Max: " + bounds.max + "\n Min: " + bounds.min);

        float maxExtent = bounds.extents.magnitude;

        initialScale = (Vector3.one * configuration.defaultObjectSize) / (2 * maxExtent);

        target.transform.localScale = initialScale;
    }

    private void EnableInputEvents()
    {
        if (!initialized || inputEventsEnabled)
        {
            Debug.LogWarning($"{typeof(ARViewerContentController).Name}: Can't subscribe because initialization is incomplete or subscription is complete.");
            return;
        }

        ViewerCameraControllerTarget.OnTargetActivated += OnTargetActivated;

        switch (configuration.trackingMode)
        {
            case ARViewerConfiguration.TrackingMode.None:
                Debug.LogWarning($"{typeof(ARViewerContentController).Name}: Can't subscribe because tracking mode is set to none.");
                return;
            case ARViewerConfiguration.TrackingMode.SurfaceTracking:

                BasicInputController.ev_tap_performed += PlaceContentOnSurface;

                BasicInputController.ev_touch_begin += ShowWorldUI;
                BasicInputController.ev_touch_finished += HideWorldUI;

                BasicInputController.ev_twist_performed += Rotate;
                BasicInputController.ev_drag_started += PanInit;
                BasicInputController.ev_drag_performed += Pan;
                BasicInputController.ev_pinch_performed += Scale;

                break;
            case ARViewerConfiguration.TrackingMode.ImageTracking:

                BasicInputController.ev_twofingerdrag_started += PanInit;
                BasicInputController.ev_twofingerdrag_performed += Pan;
                BasicInputController.ev_drag_performed += Rotate;
                BasicInputController.ev_pinch_performed += Scale;

                break;
            default:
                break;
        }

        inputEventsEnabled = true;
    }

    private void DisableInputEvents()
    {
        if (!initialized || !inputEventsEnabled)

            if (!initialized)
            {
                Debug.LogWarning($"{typeof(ARViewerSceneController).Name}: Can't unsubscribe because initialization is incomplete or subscription is incomplete.");
                return;
            }

        ViewerCameraControllerTarget.OnTargetActivated -= OnTargetActivated;

        switch (configuration.trackingMode)
        {
            case ARViewerConfiguration.TrackingMode.None:
                Debug.LogWarning($"{typeof(ARViewerSceneController).Name}: Can't unsubscribe because tracking mode is set to none.");
                return;
            case ARViewerConfiguration.TrackingMode.SurfaceTracking:

                BasicInputController.ev_tap_performed -= PlaceContentOnSurface;

                BasicInputController.ev_touch_begin -= ShowWorldUI;
                BasicInputController.ev_touch_finished += HideWorldUI;

                BasicInputController.ev_twist_performed -= Rotate;
                BasicInputController.ev_drag_started -= PanInit;
                BasicInputController.ev_drag_performed -= Pan;
                BasicInputController.ev_pinch_performed -= Scale;

                break;
            case ARViewerConfiguration.TrackingMode.ImageTracking:

                BasicInputController.ev_twofingerdrag_started -= PanInit;
                BasicInputController.ev_twofingerdrag_performed -= Pan;
                BasicInputController.ev_drag_performed -= Rotate;
                BasicInputController.ev_pinch_performed -= Scale;

                break;
            default:
                break;
        }

        inputEventsEnabled = false;
    }

    private void OnTargetActivated(ViewerCameraControllerTarget target) => currentTarget = target;

    private void OnARViewerSceneControllerInitializationComplete(ARViewerSceneController controller) => Initialize(controller.configuration);
}

#endif