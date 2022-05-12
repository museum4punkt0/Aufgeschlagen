using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace com.guidepilot.guidepilotcore
{
    

    [GlobalConfig("Assets/Resources")]
    public class CustomGlobalDefinesConfig : GlobalConfig<CustomGlobalDefinesConfig>
    {
        //[Serializable]
        //public class Submodule
        //{
        //    public enum Status { Enabled, Disabled };
        //
        //    [EnumToggleButtons]
        //    public Status status = Status.Enabled;
        //
        //    [Flags]
        //    public enum TargetGroup
        //    {               
        //        Standalone = 1 << 1,
        //        Android = 1 << 2,
        //        iOS = 1 << 3,
        //        All = Standalone | Android | iOS
        //    }
        //
        //    [HideIf("status", Status.Disabled)]
        //    [EnumToggleButtons]
        //    public TargetGroup targetGroups = TargetGroup.All;
        //}
        //
        //[InlineEditor]
        //public Submodule inputSubmodule;
        //
        //public Submodule viewer3dSubmodule;
        //
        //public Submodule localizationSubmodule;
        //
        //public Submodule augmentedRealitySubmodule;

        public enum GlobalDefineStatus { Enabled, Disabled }

        [EnumToggleButtons]
        [SerializeField]
        private GlobalDefineStatus essentials;

        [EnumToggleButtons]
        [SerializeField]
        private GlobalDefineStatus input;

        [EnumToggleButtons]
        [SerializeField]
        private GlobalDefineStatus viewer3D;

        [EnumToggleButtons]
        [SerializeField]
        private GlobalDefineStatus localization;

        [EnumToggleButtons]
        [SerializeField]
        private GlobalDefineStatus augmentedReality;

        [ReadOnly]
        [SerializeField]
        private BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[] { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };

        public const string EssentialsDefineSymbol = "GUIDEPILOT_CORE_ESSENTIALS";
        public const string InputDefineSymbol = "GUIDEPILOT_CORE_INPUT";
        public const string Viewer3DDefineSymbol = "GUIDEPILOT_CORE_VIEWER3D";
        public const string LocalizationDefineSymbol = "GUIDEPILOT_CORE_LOCALIZATION";
        public const string AugmentedRealityDefineSymbol = "GUIDEPILOT_CORE_AR";

        [OnInspectorInit]
        private void ValidateDefines()
        {
            //List<BuildTargetGroup> inputTargetGroups = GetBuildTargetGroups(inputSubmodule.targetGroups);
            //
            //foreach (BuildTargetGroup targetGroup in inputTargetGroups)
            //{
            //    // Get Symbols for specific build target group
            //    string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            //
            //    Submodule.TargetGroup submoduleTargetGroup = Submodule.TargetGroup.Standalone;
            //
            //    inputSubmodule.targetGroups = (ContainsExactString(symbols, InputDefineSymbol)) ? inputSubmodule.targetGroups |= Submodule.TargetGroup.Standalone : inputSubmodule.targetGroups &= ~Submodule.TargetGroup.Standalone;  
            //}

            string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

            essentials = ContainsExactString(currentSymbols, EssentialsDefineSymbol) ? essentials = GlobalDefineStatus.Enabled : essentials = GlobalDefineStatus.Disabled;
            input = ContainsExactString(currentSymbols, InputDefineSymbol) ? input = GlobalDefineStatus.Enabled : input = GlobalDefineStatus.Disabled;
            viewer3D = ContainsExactString(currentSymbols, Viewer3DDefineSymbol) ? viewer3D = GlobalDefineStatus.Enabled : viewer3D = GlobalDefineStatus.Disabled;
            localization = ContainsExactString(currentSymbols, LocalizationDefineSymbol) ? localization = GlobalDefineStatus.Enabled : localization = GlobalDefineStatus.Disabled;
            augmentedReality = ContainsExactString(currentSymbols, AugmentedRealityDefineSymbol) ? augmentedReality = GlobalDefineStatus.Enabled : augmentedReality = GlobalDefineStatus.Disabled;
        }

        [Button(ButtonSizes.Large, Name = "Apply Changes")]
        private void UpdateGlobalDefines()
        {
            Dictionary<string, bool> defineActionDictionary = new Dictionary<string, bool>();

            defineActionDictionary[EssentialsDefineSymbol] = (essentials == GlobalDefineStatus.Enabled);
            defineActionDictionary[InputDefineSymbol] = (input == GlobalDefineStatus.Enabled);
            defineActionDictionary[Viewer3DDefineSymbol] = (viewer3D == GlobalDefineStatus.Enabled);
            defineActionDictionary[LocalizationDefineSymbol] = (localization == GlobalDefineStatus.Enabled);
            defineActionDictionary[AugmentedRealityDefineSymbol] = (augmentedReality == GlobalDefineStatus.Enabled);

            foreach (var targetGroup in buildTargetGroups)
            {
                foreach (var entry in defineActionDictionary)
                {
                    if (entry.Value)
                    {
                        AddGlobalDefine(entry.Key, targetGroup);
                    }
                    else
                    {
                        RemoveGlobalDefine(entry.Key, targetGroup);
                    }
                }
            }
        }

        private void AddGlobalDefine(string symbol, BuildTargetGroup targetGroup)
        {
            string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            if (!ContainsExactString(currentSymbols, symbol))
            {
                string newSymbol = (currentSymbols.Length > 0) ? $";{symbol}" : symbol;
                currentSymbols += newSymbol;

                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, currentSymbols);

                Debug.Log($"{typeof(CustomGlobalDefinesConfig)}: Added globale define ({symbol}) to {targetGroup}");
            }
        }

        private void RemoveGlobalDefine(string symbol, BuildTargetGroup targetGroup)
        {
            string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            if (ContainsExactString(currentSymbols, symbol))
            {
                string newSymbol = (currentSymbols.Length > 0) ? $";{symbol}" : symbol;
                currentSymbols = currentSymbols.Replace(newSymbol, string.Empty);

                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, currentSymbols);

                Debug.Log($"{typeof(CustomGlobalDefinesConfig)}: Removed globale define ({symbol}) from {targetGroup}");
            }
        }

        private bool ContainsExactString(string input, string pattern)
        {
            return Regex.IsMatch(input, $@"\b{pattern}\b", RegexOptions.IgnoreCase);
        }

        //[Button]
        //private List<BuildTargetGroup> GetBuildTargetGroups(Submodule.TargetGroup targetGroup)
        //{
        //    List<BuildTargetGroup> buildTargetGroups = new List<BuildTargetGroup>();
        //
        //    switch (targetGroup)
        //    {
        //        case Submodule.TargetGroup.Standalone:
        //            buildTargetGroups.Add(BuildTargetGroup.Standalone);
        //            break;
        //        case Submodule.TargetGroup.Android:
        //            buildTargetGroups.Add(BuildTargetGroup.Android);
        //            break;
        //        case Submodule.TargetGroup.iOS:
        //            buildTargetGroups.Add(BuildTargetGroup.iOS);
        //            break;
        //        case Submodule.TargetGroup.All:
        //            buildTargetGroups.Add(BuildTargetGroup.Standalone);
        //            buildTargetGroups.Add(BuildTargetGroup.Android);
        //            buildTargetGroups.Add(BuildTargetGroup.iOS);
        //            break;
        //        default:
        //            break;
        //    }
        //
        //    return buildTargetGroups;
        //}
    }
}


