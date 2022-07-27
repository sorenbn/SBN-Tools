using SBN.UITool.Core.Elements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBN.UITool.Core.Managers
{
    /// <summary>
    /// The general UI Manager to control window changing
    /// modal popup and UI Element animation state. 
    /// 
    /// There should only exist one of these for the entire lifecycle of the project.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIWindowContainer windowContainer;

        [Header("Settings")]
        [SerializeField] private UIWindow initialWindow;
        [SerializeField] private List<UIWindow> preloadWindows;
        private Stack<UIWindow> windowHistory = new Stack<UIWindow>();

        private Dictionary<UIWindowId, UIWindow> allWindows = new Dictionary<UIWindowId, UIWindow>();

        public UIModalManager ModalManager { get;
            private set; }
        public UIWindow CurrentWindow { get;
            private set; }

        private void Awake()
        {
            ModalManager = GetComponent<UIModalManager>();

            SetupPreloadedWindows();
            DontDestroyOnLoad(gameObject);
        }

        private void SetupPreloadedWindows()
        {
            for (int i = 0; i < preloadWindows.Count; i++)
            {
                var screen = preloadWindows[i];
                var element = Instantiate(screen, transform);

                element.Setup(this);
                element.HideInstant();

                allWindows.Add(screen.Id, element);
            }
        }

        private void Start()
        {
            if (initialWindow != null)
                ShowWindow(initialWindow.Id);
        }

        private void OnEnable()
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
        }

        public void ShowWindow(UIWindowId windowId)
        {
            if (CurrentWindow != null && CurrentWindow?.Id == windowId)
                return;

            CurrentWindow?.Hide();

            if (windowId == UIWindowId.None)
            {
                HideAllWindows();
                return;
            }

            if (!allWindows.TryGetValue(windowId, out var target))
            {
                var nextWindow = Instantiate(windowContainer.GetWindowById(windowId), transform);
                nextWindow.Setup(this);

                target = nextWindow;
                allWindows.Add(windowId, nextWindow);
            }

            CurrentWindow = target;
            CurrentWindow.Show();

            windowHistory.Push(CurrentWindow);
        }

        public void HideWindow(UIWindowId windowId)
        {
            if (!allWindows.TryGetValue(windowId, out var target))
            {
                var nextWindow = Instantiate(windowContainer.GetWindowById(windowId), transform);
                nextWindow.Setup(this);

                target = nextWindow;
                allWindows.Add(windowId, nextWindow);
            }

            target.Hide();
        }

        public void HideAllWindows()
        {
            CurrentWindow?.HideInstant();
            CurrentWindow = null;

            ClearHistory();
        }

        public void GoBack()
        {
            if (windowHistory.Count == 0)
                return;

            var nextWindow = windowHistory.Pop();
            ShowWindow(nextWindow.Id);
        }

        public void ClearHistory()
        {
            windowHistory.Clear();
        }

        private void UnloadWindows()
        {
            var unloadWindows = allWindows
            .Where(x => !x.Value.GetSettings().DontDetroyOnLoad)
            .ToList();

            foreach (var window in unloadWindows)
            {
                allWindows.Remove(window.Key);
                Destroy(window.Value.gameObject);
            }

            HideAllWindows();
        }

        private void SceneManager_sceneUnloaded(Scene scene)
        {
            UnloadWindows();
        }
    }
}