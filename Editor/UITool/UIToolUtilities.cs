using SBN.UITool.Core.Elements.Windows;
using SBN.UITool.Core.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SBN.EditorTools.UITool
{
    public static class UIToolUtilities
    {
        [MenuItem("SBN/UI Tool/Create UI Manager")]
        [MenuItem("GameObject/SBN/UI Tool/Create UI Manager")]
        public static void CreateUIManager()
        {
            var existingManager = Object.FindObjectOfType<UIManager>();

            if (existingManager != null)
            {
                Debug.LogError($"There is already a UI Manager in the scene and there should only ever be one.", existingManager);
                return;
            }

            var go = new GameObject("UIManager");
            var uiManager = go.AddComponent<UIManager>();

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var canvasScaler = go.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.matchWidthOrHeight = 0.5f;

            var eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.transform.SetParent(go.transform);
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();

            SBNEditorUtilities.MoveComponentToTop(go, uiManager);
        }

        [MenuItem("SBN/UI Tool/Create UI Window")]
        [MenuItem("GameObject/SBN/UI Tool/Create UI Window")]
        public static void CreateUIWindow()
        {
            var existingManager = Object.FindObjectOfType<UIManager>();

            if (existingManager == null)
            {
                Debug.LogError($"Please make sure a UI Manager is present in the scene before adding a UI Window");
                return;
            }

            var go = new GameObject("UIWindow_NAME");
            var window = go.AddComponent<UIWindow>();
            go.AddComponent<GraphicRaycaster>();

            go.transform.SetParent(existingManager.transform, false);
            go.transform.localScale = Vector3.one;

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            SBNEditorUtilities.MoveComponentToTop(go, window);

            Selection.SetActiveObjectWithContext(go, go);
        }
    }
}