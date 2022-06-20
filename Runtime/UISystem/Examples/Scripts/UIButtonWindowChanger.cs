using SBN.UITool.Core.Elements;
using UnityEngine;

public class UIButtonWindowChanger : UIElement 
{
    [SerializeField] private UIWindowId targetWindow;

    public void ShowWindow()
    {
        UIManager.ShowWindow(targetWindow);
    }
}