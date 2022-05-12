#if GUIDEPILOT_CORE_ESSENTIALS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace com.guidepilot.guidepilotcore
{
    [CreateAssetMenu(fileName = "UnityContent", menuName = "Guidepilot/Content/UnityContent")]
    public class UnityContent : ScriptableObject
    {
        public enum Type
        {
            None,
            UnityContent,
            POITarget,
            POILayer,
            POI
        }

        public enum Title
        {
            Title,
            Subtitle,
            Description
        }

        [BoxGroup("General")]
        [ToggleGroup("General/editGeneralSettings", "Edit General Settings")]
        public bool editGeneralSettings = false;

        [ToggleGroup("General/editGeneralSettings")]
        public string contentSUUID = null;

        [ToggleGroup("General/editGeneralSettings")]
        public string contentUsageSUUID = null;

        [ToggleGroup("General/editGeneralSettings")]
        public bool canInvokeUnityCommandTarget = false;

        [BoxGroup("Titles")]
        public bool enableLocalization = false;

        [BoxGroup("Titles")]
        [HideIf("enableLocalization")]
        public string title = "Insert title here";

        [BoxGroup("Titles")]
        [DrawWithUnity]
        [ShowIf("enableLocalization")]
        public LocalizedString titleReference = new LocalizedString();

        [BoxGroup("Titles")]
        [HideIf("enableLocalization")]
        public string subtitle = "Insert subtitle here";

        [BoxGroup("Titles")]
        [DrawWithUnity]
        [ShowIf("enableLocalization")]
        public LocalizedString subtitleReference = new LocalizedString();

        [BoxGroup("Titles")]
        [HideIf("enableLocalization")]
        [TextArea(3, 5)]
        [HideLabel]
        public string description = "Insert description here";

        [BoxGroup("Titles")]
        [DrawWithUnity]
        [ShowIf("enableLocalization")]
        public LocalizedString descriptionReference = new LocalizedString();

        [BoxGroup("Image")]
        public Sprite sprite = null;

        [BoxGroup("Image")]
        public AssetReferenceAtlasedSprite spriteReference = default;

        [BoxGroup("Content")]
        [ReadOnly]
        public UnityContent parentContent = null;

        [BoxGroup("Content")]
        [ReadOnly]
        [ListDrawerSettings(Expanded = true)]
        public List<UnityContent> childContent = new List<UnityContent>();

        public static UnityAction<UnityContent> OnUnityContentInvokeEvent;

        public void InvokeUnityCommandTarget() => OnUnityContentInvokeEvent?.Invoke(this);
        //{
        //    if (NativeBridge.Instance == null)
        //    {
        //        Debug.LogWarning($"{this.name}: Can't invoke Unity Command Target because Native Bridge is not initialized.");
        //        return;
        //    }
        //    
        //    string targetString = NativeBridge.GetTargetString(contentUsageSUUID, TargetType.ContentUsage);
        //    NativeBridge.Instance.unityCommandTarget(targetString);
        //    Debug.Log($"{this.name}: Invoke Unity Command Target: {targetString}");
        //}

    }

}
#endif
