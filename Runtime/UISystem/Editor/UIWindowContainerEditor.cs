using SBN.UITool.Core;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SBN.UITool.EditorTool
{
    [CustomEditor(typeof(UIWindowContainer))]
    public class UIWindowContainerEditor : Editor
    {
        private const string enumName = "UIWindowId";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var scriptTarget = (UIWindowContainer)target;

            EditorGUILayout.LabelField("Step 1");

            if (GUILayout.Button("Generate Ids"))
                GenerateUIWindowIds(scriptTarget);

            EditorGUILayout.LabelField("Step 2");

            if (GUILayout.Button("Assign Ids"))
                AssignIds(scriptTarget);
        }

        public void GenerateUIWindowIds(UIWindowContainer scriptTarget)
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
                outfile.WriteLine($"public enum {enumName}");
                outfile.WriteLine("{ ");
                outfile.WriteLine("None = 0,");

                var windows = scriptTarget.GetAllWindows();

                for (int i = 0; i < windows.Length; i++)
                    outfile.WriteLine($"{windows[i].name.Replace(" ", "").Replace("_", "")} = {i + 1},");

                outfile.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

        public void AssignIds(UIWindowContainer scriptTarget)
        {
            var windows = scriptTarget.GetAllWindows();

            for (int i = 0; i < windows.Length; i++)
                windows[i].Id = (UIWindowId)(i + 1);
        }
    } 
}