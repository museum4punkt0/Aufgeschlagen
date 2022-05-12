using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;


namespace com.guidepilot.guidepilotcore
{
    public class CustomGlobalDefinesEditorWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private Editor editor;

        [MenuItem("MicroMovie/Update Globale Defines")]
        private static void OpenWindow()
        {
            GetWindow<CustomGlobalDefinesEditorWindow>("Update Globale Defines").Show();
        }

        private void OnEnable()
        {
            this.editor = Editor.CreateEditor(CustomGlobalDefinesConfig.Instance);

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
