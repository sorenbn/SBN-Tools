using SBN.UITool.Core.Elements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBN.UITool.Core
{
    public class UIWindowManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIWindowContainer windowContainer;

        [Header("Settings")]
        [SerializeField] private UIWindow initialWindow;
        [SerializeField] private List<UIWindow> preloadWindows;

        private UIWindow currentWindow;
        private Stack<UIWindow> windowHistory = new Stack<UIWindow>();

        private Dictionary<UIWindowId, UIWindow> allWindows = new Dictionary<UIWindowId, UIWindow>();

        private void Awake()
        {
            for (int i = 0; i < preloadWindows.Count; i++)
            {
                var screen = preloadWindows[i];
                var element = Instantiate(screen, transform);

                element.Setup(this);
                element.HideInstant();

                allWindows.Add(screen.Id, element);
            }

            DontDestroyOnLoad(gameObject);
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

        // TEST METHOD, DELETE ME LATER
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowWindow(UIWindowId.WindowMainMenu);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShowWindow(UIWindowId.WindowOptions);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShowWindow(UIWindowId.None);
            }
        }

        public void ShowWindow(UIWindowId windowId)
        {
            if (currentWindow != null && currentWindow?.Id == windowId)
                return;

            currentWindow?.Hide();

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

            currentWindow = target;
            currentWindow.Show();

            windowHistory.Push(currentWindow);
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
            currentWindow?.HideInstant();
            currentWindow = null;

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

        private void SceneManager_sceneUnloaded(Scene arg0)
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
    }
}