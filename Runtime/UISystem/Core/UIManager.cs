using System.Collections.Generic;
using UnityEngine;

namespace SBN.UITool.Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIScreen initialScreen;
        [SerializeField] private List<UIScreen> preloadScreens;

        // TODO: tmp, until history is implemented.
        private UIElement currentElement;

        private Dictionary<UIScreen, UIElement> allScreens = new Dictionary<UIScreen, UIElement>();

        private void Awake()
        {
            for (int i = 0; i < preloadScreens.Count; i++)
            {
                var screen = preloadScreens[i];
                var element = Instantiate(screen.Element, transform);
                element.HideInstant();

                allScreens.Add(screen, element);
            }
        }

        private void Start()
        {
            if (initialScreen != null)
                ShowScreen(initialScreen);
        }

        public void ShowScreen(UIScreen screen)
        {
            currentElement?.Hide();

            if (!allScreens.TryGetValue(screen, out var target))
            {
                target = Instantiate(screen.Element, transform);
                allScreens.Add(screen, target);
            }

            currentElement = target;
            currentElement.Show();
        }

        public void HideScreen(UIScreen screen)
        {
            if (!allScreens.TryGetValue(screen, out var target))
            {
                target = Instantiate(screen.Element, transform);
                allScreens.Add(screen, target);
            }

            currentElement = target;
            currentElement.Hide();
        }
    }
}