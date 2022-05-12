#if GUIDEPILOT_CORE_VIEWER3D

using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace com.guidepilot.guidepilotcore
{
    [System.Serializable]
    public class ViewerScene
    {
        public enum LoadingMode { SceneManager, UnityEvent }

        public UnityContent content;
        public string sceneName;
        public LoadingMode loadingMode;

        //public class ViewerSceneEvent : UnityEvent<ViewerScene> {};
        public static UnityAction<ViewerScene> OnLoadEvent;
        public static UnityAction<ViewerScene> OnUnloadEvent;

        [Button("Load Scene")]
        public void Load()
        {
            OnLoadEvent?.Invoke(this);
        }

        [Button("Unload Scene")]
        public void Unload()
        {
            OnUnloadEvent?.Invoke(this);
        }
    }
}

#endif
