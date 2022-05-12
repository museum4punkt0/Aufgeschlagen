#if GUIDEPILOT_CORE_ESSENTIALS

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO.Compression;
using System.IO;

using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using UnityEditor.AddressableAssets.Settings;


namespace com.guidepilot.guidepilotcore
{
    [GlobalConfig("Assets/CommonScripts/Editor")]
    public class CMSDataImporterConfig : GlobalConfig<CMSDataImporterConfig>
    {
        [Serializable]
        public class DataLayer
        {
            [OnValueChanged("CreateContentInstance")]
            [OnValueChanged("CreateFolderName")]
            public UnityContent.Type contentType;
            public string folderName = null;

            [InlineEditor]
            public UnityContent content = null;

            private void CreateContentInstance()
            {
                if (content != null)
                    DestroyImmediate(content);

                switch (contentType)
                {
                    case UnityContent.Type.None:
                        if (content != null)
                            Destroy(content);
                        break;
                    case UnityContent.Type.UnityContent:
                        content = CreateInstance<UnityContent>();
                        break;
#if GUIDEPILOT_CORE_AR
                    case UnityContent.Type.POITarget:
                        content = CreateInstance<POITarget>();
                        break;
                    case UnityContent.Type.POILayer:
                        content = CreateInstance<POILayer>();
                        break;
                    case UnityContent.Type.POI:
                        content = CreateInstance<POI>();
                        break;
#endif
                    default:
                        break;
                }
            }

            private void CreateFolderName()
            {
                if (folderName == string.Empty || contentType == UnityContent.Type.None)
                {
                    folderName = contentType != UnityContent.Type.None ? $"{this.contentType.ToString()}s" : string.Empty;
                }

            }
        }

        [Serializable]
        public class TitleConversion
        {
            [HorizontalGroup("Horizontal")]
            [LabelText("From")]
            public Title fromTitle;

            [HorizontalGroup("Horizontal")]
            [LabelText("to")]
            public UnityContent.Title toTitle;
        }

        public enum Title
        {
            Title,
            Subtitle,
            TitleShort,
            SubtitleShort
        }

        [ToggleGroup("mainPackageImportSettings", "Edit Main Package Import Settings")]
        [SerializeField]
        private bool mainPackageImportSettings = false;

        [ToggleGroup("mainPackageImportSettings", "Edit Main Package Import Settings")]
        [SerializeField]
        private string webHeaderName = null;

        [ToggleGroup("mainPackageImportSettings", "Edit Main Package Import Settings")]
        [SerializeField]
        private string webHeaderValue = null;

        [ToggleGroup("mainPackageImportSettings", "Edit Main Package Import Settings")]
        [SerializeField]
        private string webClientAddress = null;

        [ToggleGroup("mainPackageImportSettings", "Edit Main Package Import Settings")]
        [FolderPath]
        [SerializeField]
        private string mainPackageFolder = null;

        [BoxGroup("Settings")]
        [SerializeField]
        private string compilationSUUID = null;

        [BoxGroup("Settings")]
        [FolderPath]
        [SerializeField]
        private string rootFolder = null;

        [BoxGroup("Settings")]
        [SerializeField]
        private string projectName = null;

        [ToggleGroup("enableLocalization", "Enable Localization")]
        [SerializeField]
        private bool enableLocalization = false;

        [ToggleGroup("enableLocalization", "Enable Localization")]
        [SerializeField]
        private string tableCollectionName = null;

        [ToggleGroup("enableLocalization", "Enable Localization")]
        [FolderPath]
        [SerializeField]
        private string tableCollectionPath = null;

        [ToggleGroup("enableLocalization", "Enable Localization")]
        [SerializeField]
        [ListDrawerSettings(DraggableItems = false)]
        private List<TitleConversion> titleConversions = new List<TitleConversion>();

        [BoxGroup("Content")]
        [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
        [SerializeField]
        private List<DataLayer> dataLayers = new List<DataLayer>();

        private Package mainPackage = null;

        private Dictionary<ContentUsage, string> fileNamePrefixDictionary = new Dictionary<ContentUsage, string>();
        private Dictionary<ContentUsage, UnityContent> unityContentDictionary = new Dictionary<ContentUsage, UnityContent>();
        private ContentCompilation currentCompilation;
        private ContentUsage[] allUsagesOfCurrentCompilation;        

        [Button(ButtonSizes.Gigantic)]
        public void ImportCMSData()
        {
            if (!ImportSettingsValid()) return;

            //TODO: Implement Update Main Package functionality
            UpdateMainPackage();
            ReadMainPackage();

            // Get Compilation
            if (!TryFindCompilation(compilationSUUID, out ContentCompilation compilation)) return;

            currentCompilation = compilation;

            // Get all content usages that match compilation id
            allUsagesOfCurrentCompilation = Array.FindAll(mainPackage.data.contentUsages, usage => usage.contentCompilationId == currentCompilation.id);

            List<ContentUsage> usages = Array.FindAll(allUsagesOfCurrentCompilation, usage => usage.parentContentUsageId == default).ToList();
            usages.OrderBy(usage => usage.priority);

            fileNamePrefixDictionary.Clear();
            unityContentDictionary.Clear();

            foreach (DataLayer dataLayer in dataLayers)
            {
                if (dataLayer.contentType != UnityContent.Type.None)
                {
                    if (!TryCreateFolder(rootFolder, dataLayer.folderName)) continue;
                }

                string folderPath = $"{rootFolder}/{dataLayer.folderName}";
                List<ContentUsage> childUsages = new List<ContentUsage>();

                foreach (ContentUsage usage in usages)
                {
                    ContentUsage parentUsage = Array.Find(allUsagesOfCurrentCompilation, parentUsage => parentUsage.id == usage.parentContentUsageId);
                    ContentUsage[] childUsagesOfCurrentUsage = Array.FindAll(allUsagesOfCurrentCompilation, childUsage => childUsage.parentContentUsageId == usage.id);
                    childUsages.AddRange(childUsagesOfCurrentUsage);

                    Content content = Array.Find(mainPackage.data.contents, content => content.id == usage.contentId);

                    string fileName = CreateFileName(usage, parentUsage, dataLayer.contentType);

                    if (dataLayer.contentType != UnityContent.Type.None && dataLayer.content != null)
                    {
                        UnityContent unityContent = CreateUnityContent(dataLayer.content, fileName, folderPath);

                        if (unityContent != null)
                        {
                            PopulateUnityContent(unityContent, content, usage, parentUsage);
                        }
                    }
                }

                usages = childUsages;
            }

            AssetDatabase.SaveAssets();

            Debug.Log($"{typeof(CMSDataImporterConfig)}: CMS Data successfully imported!");
        }

        private void PopulateUnityContent(UnityContent unityContent, Content content, ContentUsage usage, ContentUsage parentUsage)
        {
            unityContent.contentUsageSUUID = usage.suuid;
            unityContent.contentSUUID = content.suuid;
            unityContentDictionary[usage] = unityContent;

            bool hasContentDocuments = content.contentDocuments != null;
            unityContent.canInvokeUnityCommandTarget = hasContentDocuments;

            ConvertTitles(unityContent, content);

            if (parentUsage != null && unityContentDictionary.ContainsKey(parentUsage))
            {
                UnityContent parentUnityContent = unityContentDictionary[parentUsage];
                unityContent.parentContent = parentUnityContent;

                if (parentUnityContent.childContent.Find(x => x.contentUsageSUUID == unityContent.contentUsageSUUID) == null)
                {
                    parentUnityContent.childContent.Add(unityContent);
                }
            }
        }      

        private void ConvertTitles(UnityContent unityContent, Content content)
        {
            foreach (TitleConversion conversion in titleConversions)
            {
                GetContentTitleString(content, conversion.fromTitle, out Dictionary<string, object> contentTitleStrings);
                GetUnityTitleString(unityContent, conversion.toTitle, out string unityTitleString, out LocalizedString unityTitleStringReference);

                if (contentTitleStrings != null)
                {
                    foreach (KeyValuePair<string, object> entry in contentTitleStrings)
                    {
                        if (entry.Value != null)
                        {
                            if (!unityContent.enableLocalization)
                                unityContent.enableLocalization = true;

                            string tableEntryKey = CreateTableEntryKey(content.suuid, conversion.toTitle.ToString());
                            CreateStringTableEntry(tableCollectionName, entry.Key, tableEntryKey, (string)entry.Value);
                            unityTitleStringReference.SetReference(tableCollectionName, tableEntryKey);
                        }
                        else
                        {
                            unityTitleStringReference.SetReference(null, null);
                        }
                    }

                    unityTitleString = (string)contentTitleStrings["de"];
                }
            }
        }

        private void GetUnityTitleString(UnityContent content, UnityContent.Title unityContentTitle, out string titleString, out LocalizedString titleStringReference)
        {
            switch (unityContentTitle)
            {
                case UnityContent.Title.Title:
                    titleString = content.title;
                    titleStringReference = content.titleReference;
                    break;
                case UnityContent.Title.Subtitle:
                    titleString = content.subtitle;
                    titleStringReference = content.subtitleReference;
                    break;
                case UnityContent.Title.Description:
                    titleString = content.description;
                    titleStringReference = content.descriptionReference;
                    break;
                default:
                    titleString = string.Empty;
                    titleStringReference = null;
                    break;
            }
        }

        private void GetContentTitleString(Content content, Title contentTitle, out Dictionary<string, object> contentTitleStrings)
        {
            switch (contentTitle)
            {
                case Title.Title:
                    contentTitleStrings = content.title;
                    break;
                case Title.Subtitle:
                    contentTitleStrings = content.subtitle;
                    break;
                case Title.TitleShort:
                    contentTitleStrings = content.titleShort;
                    break;
                case Title.SubtitleShort:
                    contentTitleStrings = content.subtitleShort;
                    break;
                default:
                    contentTitleStrings = null;
                    break;
            }
        }

        private bool TryFindCompilation(string compilationSUUID, out ContentCompilation compilation)
        {
            compilation = Array.Find(mainPackage.data.contentCompilations, x => x.suuid == compilationSUUID);

            if (compilation == default)
            {
                Debug.LogWarning($"{typeof(CMSDataImporterConfig)}: No compilation with a compilation suuid of {compilationSUUID} could be found.");
                return false;
            }

            return true;
        }

        private string CreateFileName(ContentUsage usage, ContentUsage parentUsage, UnityContent.Type contentType)
        {
            if (!fileNamePrefixDictionary.ContainsKey(usage))
                fileNamePrefixDictionary[usage] = "";

            Content content = Array.Find(mainPackage.data.contents, content => content.id == usage.contentId);

            bool prefixExists = parentUsage != null && fileNamePrefixDictionary.ContainsKey(parentUsage);
            string prefix = prefixExists ? fileNamePrefixDictionary[parentUsage] : string.Empty;

            string contentTitle = $"{content.title["de"].ToString().Replace(" ", "_") }";
            string fileName = $"{projectName}_{contentType}_{prefix}_{contentTitle}";

            fileNamePrefixDictionary[usage] = $"{prefix}_{contentTitle}";

            return fileName;
        }

        private UnityContent CreateUnityContent(UnityContent content, string fileName, string folderPath)
        {
            if (content == null)
            {
                Debug.LogWarning($"{typeof(CMSDataImporterConfig)}: Can't create unity content because content is null.");
                return null;
            }

            string assetPath = $"{folderPath}/{fileName}.asset";

            UnityContent unityContent = AssetDatabase.LoadAssetAtPath(assetPath, content.GetType()) as UnityContent;
            //UnityContent[] existingUnityContent = AssetDatabase.LoadAllAssetsAtPath(assetPath) as UnityContent[];

            if (unityContent == null)
            {
                unityContent = Instantiate(content);
                unityContent.name = fileName;
                AssetDatabase.CreateAsset(unityContent, assetPath);
                AssetDatabase.SaveAssets();
            }

            return unityContent;
        }

        private bool TryCreateFolder(string folderPath, string folderName)
        {
            if (folderName == string.Empty)
            {
                Debug.LogWarning($"{typeof(CMSDataImporterConfig)}: Can't create unity content of data layer because no folder name was set.");
                return false;
            }

            if (!AssetDatabase.IsValidFolder($"{folderPath}/{folderName}"))
                AssetDatabase.CreateFolder(folderPath, folderName);


            return true;
        }

        private bool ImportSettingsValid()
        {
            bool isValid = true;

            if (compilationSUUID == string.Empty)
            {
                Debug.LogWarning($"{this.name}: Please set compilation suuid!");
                isValid = false;
            }

            if (rootFolder == string.Empty)
            {
                Debug.LogWarning($"{this.name}: Please set root folder!");
                isValid = false;
            }

            if (projectName == string.Empty)
            {
                Debug.LogWarning($"{this.name}: Please set project name!");
                isValid = false;
            }

            if (enableLocalization && tableCollectionName == string.Empty)
            {
                Debug.LogWarning($"{this.name}: Please set string table name!");
                isValid = false;
            }

            return isValid;
        }

        private void CreateStringTableEntry(string tableCollectionName, string locale, string key, string value)
        {
            StringTableCollection tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableCollectionName);

            if (tableCollection == null)
            {
                tableCollection = LocalizationEditorSettings.CreateStringTableCollection(tableCollectionName, tableCollectionPath);
                Debug.Log($"{typeof(CMSDataImporterConfig)}: Created string table collection {tableCollectionName} at {tableCollectionPath}");
            }

            StringTable stringTable = tableCollection.ContainsTable(locale) ? tableCollection.GetTable(locale) as StringTable : tableCollection.AddNewTable(locale) as StringTable;

            if (stringTable == null)
            {
                stringTable = tableCollection.AddNewTable(locale) as StringTable;
                Debug.LogWarning($"{typeof(CMSDataImporterConfig)}: Could not found a string table named {locale}");
                return;
            }

            StringTableEntry entry = stringTable.AddEntry(key, value);

            EditorUtility.SetDirty(stringTable);
            EditorUtility.SetDirty(stringTable.SharedData);
        }

        private string CreateTableEntryKey(string uniqueIdentifier, string entryType)
        {
            string entryKey = $"UNITY_{projectName.ToUpper()}_{uniqueIdentifier.ToUpper()}_{entryType.ToUpper()}";
            return entryKey;
        }

        public void UpdateMainPackage()
        {
            WebClient client = new WebClient();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2

            client.Headers.Set(webHeaderName, webHeaderValue);
            client.Encoding = System.Text.Encoding.UTF8;



            client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            using (GZipStream responseStream = new GZipStream(client.OpenRead(webClientAddress), CompressionMode.Decompress))
            {
                using (Stream s = File.Create($"{mainPackageFolder}/MainPackage.json"))// "../Export/Assets/MainPackage.json"))
                {
                    responseStream.CopyTo(s);
                }
            }
        }

        private void ReadMainPackage()
        {
            Debug.Log($"{typeof(CMSDataImporterConfig)}: Reading main package...");

            System.IO.StreamReader reader = new System.IO.StreamReader($"{mainPackageFolder}/MainPackage.json");
            mainPackage = JsonConvert.DeserializeObject<com.guidepilot.guidepilotcore.Package>(reader.ReadToEnd());
            reader.Close();
        }
        //#endif

    }
}

#endif
