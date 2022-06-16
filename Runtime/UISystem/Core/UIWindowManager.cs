using SBN.UITool.Core.Elements;
using System.Collections.Generic;
using UnityEngine;

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
        }

        private void Start()
        {
            if (initialWindow != null)
                ShowWindow(initialWindow.Id);
        }

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
            if (currentWindow?.Id == windowId)
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
                allWindows.Add(windowId, nextWindow);

                target = nextWindow;
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
                allWindows.Add(windowId, nextWindow);

                target = nextWindow;
            }

            target.Hide();
        }

        public void HideAllWindows()
        {
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
    }
}