using Cysharp.Threading.Tasks;
using SBN.Events;
using SBN.SceneLoading;
using SBN.UITool.Core.Elements.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SBN.UITool.Core.Managers
{
    /// <summary>
    /// The general UI Manager to control window changing, modal popup and UI Element animation state.
    /// There should only exist one UIManager for the entire lifecycle of the project.
    /// 
    /// Currently this system only supports one active window at a time. 
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [Header("Settings")]
        [SerializeField] private UIWindowAsset initialWindow;
        [SerializeField] private List<UIWindowAsset> preloadWindows;

        private Stack<(UIWindowAsset Asset, UIWindow Instance)> windowHistory = new Stack<(UIWindowAsset, UIWindow)>();
        private Dictionary<UIWindowAsset, UIWindow> windows = new Dictionary<UIWindowAsset, UIWindow>();

        public UIWindow CurrentWindowInstance
        {
            get;
            private set;
        }
        public UIWindowAsset CurrentWindowAsset
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (initialWindow != null)
                ShowWindow(initialWindow, gameObject.scene);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        }

        // TODO: Make ShowWindow return window?
        // TODO: Make an ShowWindowAsync which returns the windows once it's shown perhaps?
        public void ShowWindow(UIWindowAsset newWindowAsset, Scene ownerScene)
        {
            ShowWindowAsync(newWindowAsset, ownerScene).Forget();
        }

        private async UniTaskVoid ShowWindowAsync(UIWindowAsset newWindowAsset, Scene ownerScene)
        {
            if (newWindowAsset == null)
                return;

            if (CurrentWindowAsset != null)
            {
                if (CurrentWindowAsset == newWindowAsset)
                    return;

                CurrentWindowInstance.Hide();
                windowHistory.Push((CurrentWindowAsset, CurrentWindowInstance));
            }

            if (!windows.TryGetValue(newWindowAsset, out var newWindowInstance))
                newWindowInstance = await LoadWindow(newWindowAsset);

            CurrentWindowAsset = newWindowAsset;
            CurrentWindowInstance = newWindowInstance;

            newWindowInstance.Setup(this, ownerScene);
            newWindowInstance.SetFocus(true);

            newWindowInstance.Show();
        }

        private async UniTask<UIWindow> LoadWindow(UIWindowAsset newWindowAsset)
        {
            var sceneName = newWindowAsset.Scene.SceneName;
            await SceneLoader.LoadSceneAdditiveAsync(sceneName);

            var window = SceneManager.GetSceneByName(sceneName).GetRootGameObjects()[0]?.GetComponent<UIWindow>();

            if (window == null)
                throw new InvalidOperationException($"No UIWindow was found while trying to load window: {sceneName}");

            windows.Add(newWindowAsset, window);

            GlobalEvents<UIEventNewWindowLoaded>.Publish(new UIEventNewWindowLoaded
            {
                WindowAsset = newWindowAsset,
                WindowInstance = window
            });

            return window;
        }

        public void HideWindow(UIWindowAsset windowAsset)
        {
            if (!windows.TryGetValue(windowAsset, out var windowInstance))
                return;

            windowInstance.Hide();
        }

        public void HideAllWindows()
        {
            CurrentWindowInstance?.HideInstant();

            CurrentWindowInstance = null;
            CurrentWindowAsset = null;

            ClearHistory();
        }

        public void GoBack()
        {
            if (windowHistory.Count == 0)
                return;

            if (CurrentWindowAsset != null)
            {
                CurrentWindowInstance.Hide();

                CurrentWindowInstance = null;
                CurrentWindowAsset = null;
            }

            var nextWindow = windowHistory.Pop();

            ShowWindowAsync(nextWindow.Asset, nextWindow.Instance.OwnerScene).Forget();
        }

        public void ClearHistory()
        {
            windowHistory.Clear();
        }

        private async UniTaskVoid UnloadWindowsForScene(int sceneIndex)
        {
            var allWindows = windows.Select(x => (x.Key, x.Value))
                .Where(x => !x.Key.Settings.DontDestroyOnLoad)
                .ToList();

            var hierarchy = GetWindowHierarchy(sceneIndex, allWindows);

            if (hierarchy.Count == 0)
                return;

            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;

            var unloadTasks = new List<UniTask>();

            foreach (var window in hierarchy)
            {
                windows.Remove(window.WindowAsset);
                unloadTasks.Add(SceneLoader.UnloadSceneAsync(window.WindowInstance.gameObject.scene.name));
            }

            await UniTask.WhenAll(unloadTasks);

            ClearHistory();
            CurrentWindowInstance = null;
            CurrentWindowAsset = null;

            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;

            //var unloadWindows = windows
            //.Where(x => !x.Value.GetSettings().DontDestroyOnLoad && x.Value.OwnerScene.buildIndex == ownerSceneIndex)
            //.ToList();

            //if (unloadWindows.Count == 0)
            //    return;

        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            UnloadWindowsForScene(scene.buildIndex).Forget();
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode)
        {

        }

        private List<(UIWindowAsset WindowAsset, UIWindow WindowInstance)> GetWindowHierarchy(int rootSceneIndex, List<(UIWindowAsset WindowAsset, UIWindow WindowInstance)> windows)
        {
            var result = new List<(UIWindowAsset WindowAsset, UIWindow WindowInstance)>();
            var stack = new Stack<(UIWindowAsset WindowAsset, UIWindow WindowInstance)>();

            foreach (var scene in windows)
            {
                if (scene.WindowInstance.OwnerScene.buildIndex == rootSceneIndex)
                    stack.Push((scene.WindowAsset, scene.WindowInstance));
            }

            while (stack.Count > 0)
            {
                var currentWindow = stack.Pop();
                result.Add(currentWindow);

                foreach (var window in windows)
                {
                    if (window.WindowInstance.OwnerScene.buildIndex == currentWindow.WindowInstance.gameObject.scene.buildIndex)
                        stack.Push(window);
                }
            }

            return result;
        }
    }
}