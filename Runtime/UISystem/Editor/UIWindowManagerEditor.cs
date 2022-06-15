using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SBN.UITool.EditorTool
{
    public class UIManagerEditorWindow : EditorWindow
    {
        private string uiWindowResourcesPath = "";
        private string autoGenCodePath = "";

        public const int NAME = 1;

        [MenuItem("SBN/UI Tool/Window Manager")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(UIManagerEditorWindow));
            window.Show();
        }

        private void OnGUI()
        {
            uiWindowResourcesPath = EditorGUILayout.TextField("Resources path:", uiWindowResourcesPath);
            autoGenCodePath = EditorGUILayout.TextField("Auto generated code path:", autoGenCodePath);

            if (GUILayout.Button("Generate Ids"))
            {
                var prefabs = Resources.LoadAll<UIWindow>(uiWindowResourcesPath);
                GenerateUIWindowIds("UIWindowId", prefabs);
            }
        }

        public void GenerateUIWindowIds(string fileName, UIWindow[] windows)
        {
            var assets = AssetDatabase.FindAssets("UIWindowId");
            var scriptAsset = assets.FirstOrDefault();

            if (scriptAsset == null)
            {
                Debug.LogError($"ERROR: UIWindowId.cs is missing! How is your project even compiling?");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(scriptAsset);
            File.WriteAllText(path, string.Empty);

            using (StreamWriter outfile = new StreamWriter(path))
            {
                outfile.WriteLine("//THIS ENUM IS AUTO-GENERATED! DO NOT EDIT!");
                outfile.WriteLine($"public enum {fileName}");
                outfile.WriteLine("{ ");
                outfile.WriteLine("None = 0,");

                for (int i = 0; i < windows.Length; i++)
                {
                    outfile.WriteLine($"{windows[i].name.Replace(" ", "").Replace("_", "")} = {i + 1},");
                }

                outfile.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        //[DidReloadScripts]
        public void AssignIds()
        {
            Debug.Log($"done");
            var prefabs = Resources.LoadAll<UIWindow>("");

            Debug.Log($"prefabs: {prefabs.Length}");

            for (int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i].Id = (UIWindowId)(i + 1);
            }
        }
    }
}