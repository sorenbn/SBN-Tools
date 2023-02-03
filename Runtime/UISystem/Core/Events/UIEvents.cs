using SBN.UITool.Core.Elements.Windows;

public struct UIEventNewWindowLoaded
{
    public UIWindowAsset WindowAsset
    {
        get; set;
    }
    public UIWindow WindowInstance
    {
        get; set;
    }
}