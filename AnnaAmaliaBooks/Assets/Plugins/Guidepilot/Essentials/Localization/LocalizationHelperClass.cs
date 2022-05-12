#if GUIDEPILOT_CORE_ESSENTIALS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using TMPro;

namespace com.guidepilot.guidepilotcore
{
    public static class LocalizationHelperClass
    {
        public static bool InitializeLocalizationEvent<T, U>(U _target, out T initializedEvent) where U : MonoBehaviour where T : LocalizedMonoBehaviour
        {

            if (_target != null)
            {
                if (_target.TryGetComponent(out T localizationEvent))
                {
                    initializedEvent = localizationEvent;
                }
                else
                {
                    initializedEvent = _target.gameObject.AddComponent<T>();
                }

                return true;
            }
            else
            {
                initializedEvent = null;
                return false;
            }
        }

        public static bool InitializeStringLocalizationEvent(TextMeshProUGUI _target, out LocalizeStringEvent initializedEvent)
        {

            if (_target != null)
            {
                if (_target.TryGetComponent(out LocalizeStringEvent localizationEvent))
                {
                    initializedEvent = localizationEvent;
                }
                else
                {
                    initializedEvent = _target.gameObject.AddComponent<LocalizeStringEvent>();
                }

                initializedEvent.OnUpdateString.AddListener((text) => _target.text = text);

                return true;
            }
            else
            {
                initializedEvent = null;
                return false;
            }
        }

        public static void RegisterOnUpdateStringListener(LocalizeStringEvent stringEvent, UnityAction<string> onUpdateAction)
        {
            stringEvent.OnUpdateString.RemoveListener(onUpdateAction);
            stringEvent.OnUpdateString.AddListener(onUpdateAction);
        }

        public static bool StringReferenceExists(LocalizedString stringReference)
        {
            LocalizedStringDatabase database = LocalizationSettings.StringDatabase;
            StringTable table = database.GetTable(stringReference.TableReference);

            if (table != null)
            {
                StringTableEntry entry = table.GetEntry(stringReference.TableEntryReference.Key);

                if (entry != null)// || string.IsNullOrEmpty(entry.GetLocalizedString()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

#endif
