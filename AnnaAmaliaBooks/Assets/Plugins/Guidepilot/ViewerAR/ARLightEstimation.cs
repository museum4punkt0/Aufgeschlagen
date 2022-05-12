using System.Collections;
#if GUIDEPILOT_CORE_AR

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;
using Sirenix.OdinInspector;
using com.guidepilot.guidepilotcore;
using System;
using UnityEngine.Events;

//[RequireComponent(typeof(ARCameraManager))]
public class ARLightEstimation : MonoBehaviour
{
    [BoxGroup("General")]
    [SerializeField]
    private ARCameraManager cameraManager;

    [BoxGroup("General")]
    [SerializeField]
    private new Light light;

    [BoxGroup("Settings")]
    [SerializeField]
    private int minimumFrameRate = 30;

    [BoxGroup("Settings")]
    [Range(0.0f, 5.0f)]
    [SerializeField]
    private float secondsBetweenUpdate;
    
    [ToggleGroup("Settings/brightnessSettingsEnabled", "Brightness Settings")]
    [SerializeField]
    private bool brightnessSettingsEnabled;

    [ToggleGroup("Settings/brightnessSettingsEnabled")]
    [SerializeField]
    private Vector2 brightnessMinMax;

    [BoxGroup("Debug")]
    [ShowInInspector]
    private float brightness;

    [BoxGroup("Debug")]
    [ShowInInspector]
    private float colorTemperature;

    [BoxGroup("Debug")]
    [ShowInInspector]
    private Color colorCorrection;

    [BoxGroup("Debug")]
    [ShowInInspector]
    private Vector3 mainLightDirection;

    [BoxGroup("Debug")]
    [ShowInInspector]
    private Color mainLightColor;

    [BoxGroup("Debug")]
    [ShowInInspector]
    private float mainLightIntensity;

    private float lastUpdateTime;

    private void Awake()
    {
        cameraManager = FindObjectOfType<ARCameraManager>();
        this.enabled = (cameraManager != null);
    }

    private void OnEnable()
    {
        cameraManager.frameReceived += FrameChanged;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= FrameChanged;
    }

    //private void Update()
    //{
    //    if (Time.time - lastUpdateTime < secondsBetweenUpdate || 1.0f / Time.deltaTime < minimumFrameRate) return;
    //    lastUpdateTime = Time.time;
    //
    //    Debug.Log("Light Estimation Update");
    //}

    void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (Time.time - lastUpdateTime < secondsBetweenUpdate || 1.0f / Time.deltaTime < minimumFrameRate) return;

        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightness = args.lightEstimation.averageBrightness.Value;

            if (brightnessSettingsEnabled)
                brightness = Mathf.Clamp(brightness, brightnessMinMax.x, brightnessMinMax.y);

            if (light != null)
                light.intensity = brightness;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            colorTemperature = args.lightEstimation.averageColorTemperature.Value;

            if (light != null)
                light.colorTemperature = colorTemperature;
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            colorCorrection = args.lightEstimation.colorCorrection.Value;

            if (light != null)
                light.color = colorCorrection;
        }

        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            mainLightDirection = args.lightEstimation.mainLightDirection.Value;
            if (light != null)
                light.transform.rotation = Quaternion.LookRotation(mainLightDirection);

        }

        if (args.lightEstimation.mainLightColor.HasValue)
        {
            mainLightColor = args.lightEstimation.mainLightColor.Value;

            if (light != null)
                light.color = mainLightColor;
        }

        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            mainLightIntensity = args.lightEstimation.mainLightIntensityLumens.Value;
            if (light != null)
                light.intensity = args.lightEstimation.averageMainLightBrightness.Value;
        }

        lastUpdateTime = Time.time;
    }
}

#endif