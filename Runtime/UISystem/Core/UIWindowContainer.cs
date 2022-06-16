using SBN.UITool.Core.Elements;
using System.Linq;
using UnityEngine;

namespace SBN.UITool.Core
{
    [CreateAssetMenu(fileName = "UI Window Container", menuName = "SBN/UI Tool/UI Window Container")]
    public class UIWindowContainer : ScriptableObject
    {
        [SerializeField] private UIWindow[] windows;

        public UIWindow GetWindowById(UIWindowId id)
        {
            var window = windows.FirstOrDefault(x => x.Id == id);

            if (window == null)
            {
                Debug.LogError($"EEROR: No window found by id: {id}. Remember to add it to the window container.");
                return null;
            }

            return window;
        }

        public UIWindow[] GetAllWindows()
        {
            return windows;
        }
    } 
}