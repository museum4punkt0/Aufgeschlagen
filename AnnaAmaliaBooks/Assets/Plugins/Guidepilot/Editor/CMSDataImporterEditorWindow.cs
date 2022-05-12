#if GUIDEPILOT_CORE_ESSENTIALS

using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;

namespace com.guidepilot.guidepilotcore
{
    public class CMSDataImporterEditorWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private Editor editor;

        [MenuItem("MicroMovie/CMS Data Importer")]
        private static void OpenWindow()
        {
            GetWindow<CMSDataImporterEditorWindow>("CMS Data Importer").Show();
        }

        private void OnEnable()
        {
            this.editor = Editor.CreateEditor(CMSDataImporterConfig.Instance);

            AssemblyReloadEvents.afterAssemblyReload += RepaintEditor;
        }

        private void RepaintEditor()
        {
            this.editor.Repaint();
        }

        private void OnGUI()
        {
            this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);
            GUIHelper.PushHierarchyMode(false);
            this.editor.OnInspectorGUI();
            GUIHelper.PopHierarchyMode();
            EditorGUILayout.EndScrollView();

            this.RepaintIfRequested();
        }

        private void OnDestroy()
        {
            DestroyImmediate(this.editor);

            AssemblyReloadEvents.afterAssemblyReload -= RepaintEditor;
        }
    }
}

#endif
