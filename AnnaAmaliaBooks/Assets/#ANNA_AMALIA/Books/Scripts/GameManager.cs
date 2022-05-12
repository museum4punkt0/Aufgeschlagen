using com.guidepilot.guidepilotcore;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.ARFoundation;

namespace Guidepilot.KSW.FliegendeBuecher
{
    public class GameManager : SerializedMonoBehaviour
    {
        #region VARIABLES

        public enum Scene { None, Viewer, ViewerAR }

        [BoxGroup("Settings")]
        [EnumToggleButtons]
        [SerializeField]
        [ReadOnly]
        private Scene currentScene;

        [BoxGroup("Settings")]
        [SerializeField]
        private Scene loadSceneAtStart;

        [BoxGroup("Settings")]
        [InlineEditor]
        [SerializeField]
        private ViewerConfiguration viewerConfiguration;

        [BoxGroup("Settings")]
        [InlineEditor]
        [SerializeField]
        private ARViewerConfiguration ARViewerConfiguration;

        [BoxGroup("References")]
        [SerializeField]
        private Camera overlayCamera;

        [BoxGroup("References")]
        [SerializeField]
        private com.guidepilot.guidepilotcore.UIController loadingUIController;

        [BoxGroup("References")]
        [SerializeField]
        private GameObject threePointLighting;

        [BoxGroup("Content")]
        [OnValueChanged("OnCurrentContentChanged")]
        [InlineEditor]
        [SerializeField]
        private UnityContent currentContent;

        [BoxGroup("Content")]
        [Range(0f, 1)]
        [SerializeField]
        private float bookScaleInAR = 1f;

        [BoxGroup("Content")]
        [SerializeField]
        private Dictionary<UnityContent, BookController> bookDictionary = new Dictionary<UnityContent, BookController>();

        private ViewerSceneController viewer3DSceneController;
        private ARViewerSceneController viewerARSceneController;
        private ARViewerUIManager viewerARUIManager;

        private Coroutine currentLoadViewerCoroutine;
        private Coroutine currentWaitForDoubleClickCoroutine;
        private Coroutine currentBookInitializationCoroutine;
        public bool currentContentIsSelected;
        private bool supportsAR = true;
        public bool touchGesturePerformed;

        public bool tutorialHasBeenCompleted = false;

        private bool currentTargetHasBeenActivated;

        #endregion

        #region UNITY_METHODS

        private void Awake()
        {
            ViewerSceneController.OnAwakeEvent += OnViewerAwake;
            ViewerSceneController.OnStartEvent += OnViewerStart;
            ViewerSceneController.OnDestroyEvent += OnViewerDestroy;

            ViewerTutorialController.OnTutorialStopEvent += OnTutorialStop;
            ViewerTutorialController.OnTutorialCompleteEvent += OnTutorialComplete;

            ARViewerSceneController.OnAwakeEvent += OnARViewerAwake;
            ARViewerSceneController.OnDestroyEvent += OnARViewerDestroy;
            ARViewerSceneController.OnInitializationCompleteEvent += OnARViewerInitializationComplete;
            ARViewerSceneController.OnTrackingAddedEvent += OnTrackingAdded;
            ARViewerSceneController.OnTrackingFoundEvent += OnTrackingFound;

            ARViewerUIManager.OnAwakeEvent += OnARViewerUIManagerAwake;

            BookController.OnBookControllerAwakeEvent += OnBookAwaked;

            ViewerSceneController.OnViewerSceneLoadEvent += OnViewerSceneLoad;
            ViewerSceneController.OnViewerSceneUnloadEvent += OnViewerSceneUnload;

            ARViewerSceneController.OnViewerSceneLoadEvent += OnViewerSceneLoad;
            ARViewerSceneController.OnViewerSceneUnloadEvent += OnViewerSceneUnload;

            UniversalAdditionalCameraData overlayCameraData = overlayCamera.GetUniversalAdditionalCameraData();

            if (overlayCameraData.renderType != CameraRenderType.Overlay)
                overlayCameraData.renderType = CameraRenderType.Overlay;
        }

        private void OnDestroy()
        {
            ViewerSceneController.OnAwakeEvent -= OnViewerAwake;
            ViewerSceneController.OnStartEvent -= OnViewerStart;
            ViewerSceneController.OnDestroyEvent -= OnViewerDestroy;

            ViewerTutorialController.OnTutorialStopEvent -= OnTutorialStop;
            ViewerTutorialController.OnTutorialCompleteEvent -= OnTutorialComplete;

            ARViewerSceneController.OnAwakeEvent -= OnARViewerAwake;
            ARViewerSceneController.OnDestroyEvent -= OnARViewerDestroy;
            ARViewerSceneController.OnInitializationCompleteEvent -= OnARViewerInitializationComplete;
            ARViewerSceneController.OnTrackingAddedEvent -= OnTrackingAdded;
            ARViewerSceneController.OnTrackingFoundEvent -= OnTrackingFound;

            ARViewerUIManager.OnAwakeEvent -= OnARViewerUIManagerAwake;

            BookController.OnBookControllerAwakeEvent -= OnBookAwaked;

            ViewerSceneController.OnViewerSceneLoadEvent -= OnViewerSceneLoad;
            ViewerSceneController.OnViewerSceneUnloadEvent -= OnViewerSceneUnload;

            ARViewerSceneController.OnViewerSceneLoadEvent -= OnViewerSceneLoad;
            ARViewerSceneController.OnViewerSceneUnloadEvent -= OnViewerSceneUnload;
        }

        private void Start()
        {
            switch (loadSceneAtStart)
            {
                case Scene.Viewer:
                    LoadViewer();
                    break;
                case Scene.ViewerAR:
                    LoadARViewer();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region CUSTOM_METHODS

        public void UpdateContent(UnityContent content, bool updateViewerScene = true)
        {
            if (content == null) return;

            if (updateViewerScene)
            {
                ViewerScene viewerScene = null;

                switch (currentScene)
                {
                    case Scene.None:
                        break;
                    case Scene.Viewer:

                        if (viewer3DSceneController != null)
                        {
                            viewerScene = viewerConfiguration.viewerScenes.FirstOrDefault((x) => x.content == content);
                            viewer3DSceneController.LoadViewerScene(viewerScene);
                        }

                        break;
                    case Scene.ViewerAR:

                        if (viewerARSceneController != null)
                        {
                            viewerScene = ARViewerConfiguration.viewerScenes.FirstOrDefault((x) => x.content == content);
                            viewerARSceneController.LoadViewerScene(viewerScene);

                            viewerARSceneController.UpdateContent(content);
                        }

                        break;
                    default:
                        break;
                }
            }

            currentContent = content;

            Debug.Log($"{typeof(GameManager).Name}: Updated Content: {content}");
        }

        [Button(ButtonSizes.Large)]
        public void LoadViewer()
        {
            if (currentScene == Scene.Viewer)
            {
                Debug.Log($"{typeof(GameManager).Name}: Can't load 3D Viewer because it is already loaded.");
                return;
            }

            if (currentLoadViewerCoroutine != null)
                StopCoroutine(currentLoadViewerCoroutine);

            currentLoadViewerCoroutine = StartCoroutine(LoadViewerCoroutine());
        }

        private IEnumerator LoadViewerCoroutine()
        {
            if (loadingUIController != null)
            {
                Debug.Log($"{typeof(GameManager).Name}: Enabling Loading Screen");
                loadingUIController.SetActive(true);
                yield return new WaitForSeconds(2.0f);
            }

            if (LoadingManager.Instance.loadedScenes.Contains(SceneId.ViewerArBoot))
            {
                LoadingManager.Instance.UnloadScene(SceneId.ViewerArBoot);
                Debug.Log($"{typeof(GameManager).Name}: Unloading AR Viewer");
            }

            LoadingManager.Instance.LoadSceneAdditive(SceneId.ViewerBoot);
            currentScene = Scene.Viewer;
        }

        [Button(ButtonSizes.Large)]
        public void LoadARViewer()
        {
            if (currentScene == Scene.ViewerAR)
            {
                Debug.Log($"{typeof(GameManager).Name}: Can't load AR Viewer because it is already loaded.");
                return;
            }

            if (LoadingManager.Instance.loadedScenes.Contains(SceneId.ViewerBoot))
            {
                LoadingManager.Instance.UnloadScene(SceneId.ViewerBoot);
            }

            LoadingManager.Instance.LoadSceneAdditive(SceneId.ViewerArBoot);
            currentScene = Scene.ViewerAR;
        }

        private void InitializeViewer(ViewerSceneController controller)
        {
            if (controller == null) return;

            controller.Initialize(viewerConfiguration);

            viewer3DSceneController = controller;

            UpdateContent(currentContent);

            if (tutorialHasBeenCompleted)
                controller.tutorialController.StopTutorial();
            else
                controller.tutorialController.StartTutorial();

            if (threePointLighting != null)
                Instantiate(threePointLighting, controller.transform);
        }

        private void InitializeARViewer(ARViewerSceneController controller)
        {
            if (controller == null) return;

            controller.Initialize(ARViewerConfiguration);

            viewerARSceneController = controller;

            controller.UpdateContent(currentContent);

            UpdateContent(currentContent);
        }

        private IEnumerator InitializeBookController(BookController controller, ARTrackedImage trackedImage = null)
        {
            if (controller == null)
            {
                Debug.Log($"{typeof(GameManager).Name}: Can't initialize book because controller is null");
                yield break;
            }

            // This delay is needed because position of tracked image is wrong on trackedImagesAddedEvent and needs a short amount of time to be calculated correctly
            yield return new WaitForSeconds(0.1f);

            controller.transform.position = (trackedImage == null) ? Camera.main.transform.position + Camera.main.transform.forward * 2.0f : trackedImage.transform.position;
            controller.transform.localScale *= bookScaleInAR;
            controller.transform.LookAt(Camera.main.transform.position);

            controller.Appear(true);
        }

        #endregion

        #region CALLBACKS

        private void OnARViewerInitializationComplete(ARViewerSceneController controller) => controller.UpdateContent(currentContent);

        private void OnARViewerUIManagerAwake(ARViewerUIManager manager)
        {
            if (manager.imageTargetScanUIController != null)
                manager.imageTargetScanUIController.UpdateCustomButton("SkipARButton", LoadViewer);

            if (manager.alertBox != null)
                manager.alertBox.UpdateCustomButton("MoreButton", LoadViewer);


            viewerARUIManager = manager;
        }

        private void InitializeOverlayCamera()
        {
            if (overlayCamera != null)
            {
                UniversalAdditionalCameraData baseCameraData = Camera.main.GetUniversalAdditionalCameraData();

                if (baseCameraData != null)
                    baseCameraData.cameraStack.Add(overlayCamera);
            }
        }

        private void OnContentSUUIDUpdate(string suuid)
        {
            if (currentContent != null && currentContent.contentSUUID == suuid) return;

            if (suuid == null) return;

            UnityContent content = bookDictionary.FirstOrDefault((x) => x.Key.contentSUUID == suuid).Key;

            bool loadARViewer = currentScene != Scene.ViewerAR && supportsAR;

            if (loadARViewer)
                LoadARViewer();

            if (content != null)
                UpdateContent(content, !loadARViewer);

            if (viewerARUIManager != null)
            {
                viewerARUIManager.SetScanUIActive(true, false);
                viewerARUIManager.SetAlertBoxActive(false, false);
            }

            currentTargetHasBeenActivated = false;
        }

        public void OnShowHelp()
        {
            if (currentScene != Scene.Viewer || viewer3DSceneController == null || viewer3DSceneController.tutorialController == null) return;

            viewer3DSceneController.tutorialController.StartTutorial();
        }

        private void OnARViewerAwake(ARViewerSceneController controller)
        {
            InitializeARViewer(controller);
            InitializeOverlayCamera();
        }

        private void OnViewerAwake(ViewerSceneController controller)
        {
            if (loadingUIController != null)
                loadingUIController.SetActive(false, false);

            InitializeOverlayCamera();
        }

        private void OnViewerStart(ViewerSceneController controller) => InitializeViewer(controller);

        private void OnARViewerDestroy(ARViewerSceneController controller) => viewerARSceneController = null;

        private void OnTrackingAdded(ARTrackable trackable) => OnTrackingFound(trackable);

        private void OnTrackingFound(ARTrackable trackable)
        {
            if (trackable == null || !(trackable is ARTrackedImage) || currentTargetHasBeenActivated) return;

            ARTrackedImage trackedImage = (ARTrackedImage)trackable;

            if (trackedImage.referenceImage.name != currentContent.contentSUUID)
            {
                Debug.LogWarning($"{typeof(GameManager).Name}: Could not find a content uuid that matches {trackedImage.referenceImage.name}");
                return;
            }

            if (bookDictionary.TryGetValue(currentContent, out BookController controller))
            {
                if (controller != null)
                {
                    if (currentBookInitializationCoroutine != null)
                        StopCoroutine(currentBookInitializationCoroutine);

                    currentBookInitializationCoroutine = StartCoroutine(InitializeBookController(controller, trackedImage));

                    currentTargetHasBeenActivated = true;

                    if (viewerARUIManager != null)
                    {
                        viewerARUIManager.SetAlertBoxActive(true);
                        viewerARUIManager.SetScanUIActive(false);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"{typeof(GameManager).Name}: Could not find a book controller for {currentContent.name}");
            }
        }

        private void OnBookAwaked(BookController controller)
        {
            if (currentScene == Scene.Viewer)
            {
                controller.Appear();
                controller.enableBookToggle = true;
                controller.enablePageSwiping = true;
            }
            else if (currentScene == Scene.ViewerAR)
            {
                controller.Disappear(false);
                controller.enableBookToggle = false;
                controller.enablePageSwiping = false;
            }

            bookDictionary[controller.content] = controller;
        }


        private void OnViewerDestroy(ViewerSceneController controller) => viewer3DSceneController = null;

        private void OnCurrentContentChanged() => UpdateContent(currentContent);

        private void OnViewerSceneUnload(ViewerScene viewerScene) => LoadingManager.Instance?.UnloadScene(SceneExtension.SceneID(viewerScene.sceneName));

        private void OnViewerSceneLoad(ViewerScene viewerScene) => LoadingManager.Instance?.LoadSceneAdditive(SceneExtension.SceneID(viewerScene.sceneName));

        private void OnTutorialStop() => OnTutorialComplete();

        private void OnTutorialComplete()
        {
            tutorialHasBeenCompleted = true;
        }

       
        #endregion

        #region DEBUG

        [ButtonGroup("Books01")]
        [Button("Sauer Bible", ButtonSizes.Large)]
        public void LoadSauerBible()
        {
            string contentSUUID = "dd81e3f0-1dbf-4a64-8e1f-c375be9454bc";
            LoadBookBySUUID(contentSUUID);
        }

        [ButtonGroup("Books01")]
        [Button("JosephusOpera", ButtonSizes.Large)]
        public void LoadJosephusOpera()
        {
            string contentSUUID = "0215bad6-d03f-40b0-ad78-cb8b90724c6b";
            LoadBookBySUUID(contentSUUID);
        }

        [ButtonGroup("Books01")]
        [Button("Histoire De Jules Cesar", ButtonSizes.Large)]
        public void LoadHistoireDeJulesCesar()
        {
            string contentSUUID = "16cb500e-44a8-4e08-b7ab-93522ab96ea3";
            LoadBookBySUUID(contentSUUID);
        }

        [ButtonGroup("Books02")]
        [Button("De Statu Religionis", ButtonSizes.Large)]
        public void LoadDeStatuReligionis()
        {
            string contentSUUID = "6fec453d-d504-408e-9e23-068480f02446";
            LoadBookBySUUID(contentSUUID);
        }

        [ButtonGroup("Books02")]
        [Button("Synodus", ButtonSizes.Large)]
        public void LoadSynodus()
        {
            string contentSUUID = "a2eade09-adca-4fd8-83ce-421122b7b9eb";
            LoadBookBySUUID(contentSUUID);
        }

        [ButtonGroup("Books02")]
        [Button("Tabularium", ButtonSizes.Large)]
        public void LoadTabularium()
        {
            string contentSUUID = "70021c8e-de16-4b13-b308-73079da6a833";
            LoadBookBySUUID(contentSUUID);
        }

        private void LoadBookBySUUID(string suuid) => OnContentSUUIDUpdate(suuid);

        [Button("On Tracking Found")]
        private void OnTrackingFound(UnityContent content)
        {
            if (content.contentSUUID != currentContent.contentSUUID || currentTargetHasBeenActivated)
            {
                Debug.Log($"{typeof(GameManager).Name}: Content does not equal current content or has already been activated.");
                return;
            }

            if (bookDictionary.TryGetValue(currentContent, out BookController controller))
            {
                if (controller != null)
                {
                    if (currentBookInitializationCoroutine != null)
                        StopCoroutine(currentBookInitializationCoroutine);

                    currentBookInitializationCoroutine = StartCoroutine(InitializeBookController(controller));

                    currentTargetHasBeenActivated = true;

                    if (viewerARUIManager != null)
                    {
                        viewerARUIManager.SetAlertBoxActive(true);
                        viewerARUIManager.SetScanUIActive(false);
                    }
                }
            }

            ARViewerSceneController.OnTrackingFoundEvent?.Invoke(null);
        }

        [Button("On Tracking Lost")]
        private void OnTrackingLost(UnityContent content)
        {
            if (content != currentContent)
            {
                Debug.Log($"{typeof(GameManager).Name}: Content does not equal current content");
                return;
            }

            ARViewerSceneController.OnTrackingLostEvent?.Invoke(null);
        }

        #endregion
    }
}

