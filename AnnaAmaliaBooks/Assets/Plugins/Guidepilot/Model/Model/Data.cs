
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace com.guidepilot.guidepilotcore {

    public class Package {
        [JsonProperty("package_info")]
        public PackageInfo packageInfo;
        
        [JsonProperty("data")]
        public Data data;
    }

    public class Data {
        [JsonProperty("content_compilations")]
        public ContentCompilation[] contentCompilations;

        [JsonProperty("content_compilation_styles")]
        public ContentCompilationStyle[] contentCompilationStyles;

        [JsonProperty("contents")]
        public Content[] contents;

        [JsonProperty("content_usages")]
        public ContentUsage[] contentUsages;

        [JsonProperty("maps")]
        public Map[] maps;

        [JsonProperty("map_layers")]
        public MapLayer[] mapLayers;   

        [JsonProperty("annotations")]
        public Annotation[] annotations; 

        [JsonProperty("placements")]
        public Placement[] placements; 

        [JsonProperty("custom_triggers")]
        public CustomTrigger[] customTriggers; 

    /*    [JsonProperty("documents")]
        public Document[] documents;

        [JsonProperty("document_relationships")]
        public DocumentRelationship[] documentRelationships;
    */
        private Dictionary<int, ContentCompilation> cachedContentCompilations;
        public ContentCompilation GetContentCompilationByID(int id)
        { 
            if (cachedContentCompilations == null)
            {
                cachedContentCompilations = new Dictionary<int, ContentCompilation>();
                foreach (var compilation in contentCompilations)
                {
                    cachedContentCompilations.Add(compilation.id, compilation);
                }
            }
            return cachedContentCompilations[id];
        }

        public void completion() {
            
        }
    }
}