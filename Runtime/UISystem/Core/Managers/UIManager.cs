using SBN.Events;
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
                Debug.LogError($"ERROR! There should only ever be one instance of a UIManager. Destroying self..");
                Destroy(gameObject);

                return;
            }

            instance = this;

            SetupPreloadedWindows();
            DontDestroyOnLoad(gameObject);
        }

        private void SetupPreloadedWindows()
        {
            for (int i = 0; i < preloadWindows.Count; i++)
            {
                var windowObject = preloadWindows[i];
                var window = Instantiate(windowObject.Prefab, transform);

                window.Setup(this);
                window.HideInstant();

                windows.Add(windowObject, window);
            }
        }

        private void Start()
        {
            if (initialWindow != null)
                ShowWindow(initialWindow);
        }

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        }

        public void ShowWindow(UIWindowAsset newWindowAsset)
        {
            if (newWindowAsset == null)
                return;

            if (CurrentWindowAsset != null)
            {
                if (CurrentWindowAsset == newWindowAsset)
                    return;

                CurrentWindowInstance.Hide();
                windowHistory.Push((CurrentWindowAsset, CurrentWindowInstance));

                GlobalEvents<UIEventWindowHide>.Publish(new UIEventWindowHide { WindowAsset = CurrentWindowAsset });
            }

            if (!windows.TryGetValue(newWindowAsset, out var newWindowInstance))
            {
                var ownerScene = SceneManager.GetActiveScene();
                newWindowInstance = CreateUIWindowInstance(newWindowAsset, ownerScene);
            }

            CurrentWindowAsset = newWindowAsset;
            CurrentWindowInstance = newWindowInstance;

            CurrentWindowInstance.Show();
            GlobalEvents<UIEventWindowShow>.Publish(new UIEventWindowShow { WindowAsset = CurrentWindowAsset });
        }

        public void HideWindow(UIWindowAsset windowAsset)
        {
            if (!windows.TryGetValue(windowAsset, out var windowInstance))
                return;

            windowInstance.Hide();

            if (CurrentWindowInstance == windowInstance)
            {
                CurrentWindowInstance = null;
                CurrentWindowAsset = null;
            }
        }

        public void HideAllWindows()
        {
            if (CurrentWindowInstance != null)
            {
                CurrentWindowInstance.HideInstant();
                GlobalEvents<UIEventWindowHide>.Publish(new UIEventWindowHide { WindowAsset = CurrentWindowAsset });
            }

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
                GlobalEvents<UIEventWindowHide>.Publish(new UIEventWindowHide { WindowAsset = CurrentWindowAsset });

                CurrentWindowInstance = null;
                CurrentWindowAsset = null;
            }

            var nextWindow = windowHistory.Pop();

            ShowWindow(nextWindow.Asset);
        }

        public void ClearHistory()
        {
            windowHistory.Clear();
        }

        private UIWindow CreateUIWindowInstance(UIWindowAsset newWindowObject, Scene ownerScene)
        {
            var newWindowInstance = Instantiate(newWindowObject.Prefab, transform);
            newWindowInstance.Setup(this, ownerScene);

            windows.Add(newWindowObject, newWindowInstance);

            GlobalEvents<UIEventNewWindowCreated>.Publish(new UIEventNewWindowCreated
            {
                WindowAsset = newWindowObject,
                WindowInstance = newWindowInstance
            });

            return newWindowInstance;
        }

        private void UnloadWindows(Scene scene)
        {
            var unloadWindows = windows
            .Where(x => !x.Key.Settings.DontDestroyOnLoad && x.Value.OwnerScene.buildIndex == scene.buildIndex)
            .ToList();

            if (unloadWindows.Count == 0)
                return;

            foreach (var window in unloadWindows)
            {
                windows.Remove(window.Key);
                Destroy(window.Value.gameObject);
            }

            CurrentWindowInstance = null;
            CurrentWindowAsset = null;

            ClearHistory();
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            UnloadWindows(scene);
        }
    }
}