#if GUIDEPILOT_CORE_VIEWER3D

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.Linq;

namespace com.guidepilot.guidepilotcore
{
    public class ViewerSceneController : ViewerSceneControllerBase<ViewerSceneController, ViewerConfiguration>
    {
        //[BoxGroup("Settings")]
        //[SerializeField]
        //protected bool initializeOnStart;

        //[BoxGroup("Settings")]
        //[OnValueChanged("OnCurrentConfigurationChanged")]
        //[InlineEditor]
        //[SerializeField]
        //public ViewerConfiguration configuration { get; private set; }

        [BoxGroup("References")]
        [SceneObjectsOnly]
        public ViewerCameraController cameraController;

        [BoxGroup("References")]
        [SceneObjectsOnly]
        public ViewerTutorialController tutorialController;

        [BoxGroup("References")]
        [SceneObjectsOnly]
        public ViewerAudioController audioController;

        //public static UnityAction<ViewerSceneController> OnAwakeEvent;
        //public static UnityAction<ViewerSceneController> OnStartEvent;
        //public static UnityAction<ViewerSceneController> OnDestroyEvent;
        //public static UnityAction<ViewerSceneController> OnInitializationCompleteEvent;
        //public static UnityAction<ViewerScene> OnViewerSceneLoadEvent;
        //public static UnityAction<ViewerScene> OnViewerSceneUnloadEvent;

        //private ViewerScene currentViewerScene;

        //private List<UnityAction> subscribedActions = new List<UnityAction>();

        protected override void Awake()
        {
            //Instance = this;

            if (cameraController == null)
                cameraController = FindObjectOfType<com.guidepilot.guidepilotcore.ViewerCameraController>();

            if (audioController == null)
                audioController = FindObjectOfType<ViewerAudioController>();

            if (tutorialController == null)
                tutorialController = FindObjectOfType<ViewerTutorialController>();

            //ViewerScene.OnLoadEvent += LoadViewerScene;
            //ViewerScene.OnUnloadEvent += UnloadViewerScene;
            //
            //OnAwakeEvent?.Invoke(this);

            base.Awake();
        }

        //protected void OnDestroy()
        //{
        //    UnloadViewerScene(currentViewerScene);
        //
        //    ViewerScene.OnLoadEvent -= LoadViewerScene;
        //    ViewerScene.OnUnloadEvent -= UnloadViewerScene;
        //
        //    OnDestroyEvent?.Invoke(this);
        //}       

        public override void Initialize(ViewerConfiguration configuration)
        {
            if (configuration == null)
            {
                Debug.Log($"{typeof(ViewerSceneController)}: Can't initialize because configuration is null");
                return;
            }

            this.configuration = configuration;

            UpdateConfiguration(configuration);

            base.Initialize(configuration);
        }

        public void UpdateConfiguration(ViewerConfiguration configuration)
        {
            if (this.configuration != configuration)
            {
                this.configuration = configuration;
            }

            //foreach (ViewerConfiguration.ViewerScene viewerScene in configuration.viewerScenes)
            //{
            //    viewerScene.OnLoadEvent.RemoveAllListeners();
            //    viewerScene.OnLoadEvent.AddListener((x) => LoadViewerScene(x));
            //}

            if (cameraController != null)
            {
                cameraController.enableRotation = configuration.enableRotation;
                cameraController.rotationSpeed = configuration.rotationSpeed;

                cameraController.enableZoom = configuration.enableZoomScale;
                cameraController.zoomSpeed = configuration.zoomScaleSpeed;

                cameraController.enablePan = configuration.enablePan;

                cameraController.enableFokusMode = configuration.enableFokusMode;
                cameraController.fokusModeCameraDistance = configuration.fokusModeCameraDistance;

                if (cameraController.cam != null)
                    cameraController.cam.backgroundColor = configuration.backgroundColor;
            }

            Debug.Log($"{typeof(ViewerSceneController).Name}: Updated Configuration.");
        }

        //public virtual void LoadViewerScene(ViewerScene viewerScene)
        //{
        //    if (!ViewerSceneIsValid(viewerScene)) return;
        //
        //    UnloadViewerScene(currentViewerScene);
        //
        //    switch (viewerScene.loadingMode)
        //    {
        //        case ViewerScene.LoadingMode.SceneManager:
        //            SceneManager.LoadSceneAsync(viewerScene.sceneName, LoadSceneMode.Additive);
        //            break;
        //        case ViewerScene.LoadingMode.UnityEvent:
        //            OnViewerSceneLoadEvent?.Invoke(viewerScene);
        //            break;
        //        default:
        //            break;
        //    }
        //
        //    currentViewerScene = viewerScene;
        //}
        //
        //public virtual void UnloadViewerScene(ViewerScene viewerScene)
        //{
        //    if (!ViewerSceneIsValid(viewerScene)) return;
        //
        //    switch (viewerScene.loadingMode)
        //    {
        //        case ViewerScene.LoadingMode.SceneManager:
        //            SceneManager.UnloadSceneAsync(viewerScene.sceneName);
        //            break;
        //        case ViewerScene.LoadingMode.UnityEvent:
        //            OnViewerSceneUnloadEvent?.Invoke(viewerScene);
        //            break;
        //        default:
        //            break;
        //    }
        //
        //    currentViewerScene = null;
        //}

        //[Button("Update Configuration", ButtonSizes.Large)]
        //private void OnCurrentConfigurationChanged()
        //{
        //    UpdateConfiguration(configuration);
        //}

        //private bool ViewerSceneIsValid(ViewerScene viewerScene)
        //{
        //    if (viewerScene == null) return false;
        //
        //    if (configuration == null)
        //    {
        //        Debug.LogWarning($"{typeof(ViewerSceneController)}: Can't load scene because configuration is null");
        //        return false;
        //    }
        //
        //    if (!configuration.viewerScenes.Contains(viewerScene))
        //    {
        //        Debug.LogWarning($"{typeof(ViewerSceneController)}: Can't load scene because current configuration doesn't contain a scene called: {viewerScene}");
        //        return false;
        //    }
        //
        //    return true;
        //}


    }
}

#endif

