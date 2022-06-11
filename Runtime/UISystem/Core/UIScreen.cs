using UnityEngine;

namespace SBN.UITool.Core
{
    [CreateAssetMenu(fileName = "Screen_", menuName = "SBN/UI Tool/Screen")]
    public class UIScreen : ScriptableObject
    {
        [SerializeField] private UIElement element;

        public UIElement Element
        {
            get => element;
        }
    } 
}