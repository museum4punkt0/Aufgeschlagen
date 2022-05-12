#if GUIDEPILOT_CORE_VIEWER3D || GUIDEPILOT_CORE_AR

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.guidepilot.guidepilotcore
{
    public abstract class ViewerSceneControllerBase<T, U> : MonoBehaviour where T : ViewerSceneControllerBase<T, U> where U : ViewerConfigurationBase
    {
        [BoxGroup("Settings")]
        [SerializeField]
        protected bool initializeOnStart;

        [BoxGroup("Settings")]
        [InlineEditor]
        [SerializeField]
        public U configuration;

        [BoxGroup("General")]
        [OnValueChanged("OnCurrentContentChanged")]
        public UnityContent currentContent;

        public static UnityAction<T> OnAwakeEvent;
        public static UnityAction<T> OnStartEvent;
        public static UnityAction<T> OnDestroyEvent;
        public static UnityAction<T> OnInitializationCompleteEvent;

        public static UnityAction<UnityContent> OnContentUpdatedEvent;

        public static UnityAction<ViewerScene> OnViewerSceneLoadEvent;
        public static UnityAction<ViewerScene> OnViewerSceneUnloadEvent;

        protected ViewerScene currentViewerScene;

        private T instance;

        protected virtual void Awake()
        {
            instance = (T)this;

            ViewerScene.OnLoadEvent += LoadViewerScene;
            ViewerScene.OnUnloadEvent += UnloadViewerScene;

            OnAwakeEvent?.Invoke(instance);
        }

        protected virtual void Start()
        {
            if (initializeOnStart)
                Initialize(configuration);

            OnStartEvent?.Invoke(instance);
        }

        protected virtual void OnDestroy()
        {
            UnloadViewerScene(currentViewerScene);

            ViewerScene.OnLoadEvent -= LoadViewerScene;
            ViewerScene.OnUnloadEvent -= UnloadViewerScene;

            OnDestroyEvent?.Invoke(instance);
        }

        public virtual void Initialize(U configuration)
        {
            Debug.Log($"{typeof(U).Name}: Initialization complete");

            OnInitializationCompleteEvent?.Invoke(instance);
        }

        public virtual void UpdateContent(UnityContent content)
        {
            if (content == null) return;

            currentContent = content;

            OnContentUpdatedEvent?.Invoke(content);
        }

        public virtual void LoadViewerScene(ViewerScene viewerScene)
        {
            if (!ViewerSceneIsValid(viewerScene)) return;

            UnloadViewerScene(currentViewerScene);

            switch (viewerScene.loadingMode)
            {
                case ViewerScene.LoadingMode.SceneManager:
                    SceneManager.LoadSceneAsync(viewerScene.sceneName, LoadSceneMode.Additive);
                    break;
                case ViewerScene.LoadingMode.UnityEvent:
                    OnViewerSceneLoadEvent?.Invoke(viewerScene);
                    break;
                default:
                    break;
            }

            currentViewerScene = viewerScene;
        }

        public virtual void UnloadViewerScene(ViewerScene viewerScene)
        {
            if (!ViewerSceneIsValid(viewerScene)) return;

            switch (viewerScene.loadingMode)
            {
                case ViewerScene.LoadingMode.SceneManager:
                    SceneManager.UnloadSceneAsync(viewerScene.sceneName);
                    break;
                case ViewerScene.LoadingMode.UnityEvent:
                    OnViewerSceneUnloadEvent?.Invoke(viewerScene);
                    break;
                default:
                    break;
            }

            currentViewerScene = null;
        }

        protected virtual bool ViewerSceneIsValid(ViewerScene viewerScene)
        {
            if (viewerScene == null) return false;

            if (configuration == null)
            {
                Debug.LogWarning($"{typeof(T).Name}: Can't load scene because configuration is null");
                return false;
            }

            if (!configuration.viewerScenes.Contains(viewerScene))
            {
                Debug.LogWarning($"{typeof(T).Name}: Can't load scene because current configuration doesn't contain a scene called: {viewerScene}");
                return false;
            }

            return true;
        }

        private void OnCurrentContentChanged() => UpdateContent(currentContent);
    }
}

#endif