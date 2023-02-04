using SBN.UITool.Core.Elements.Windows;

public struct UIEventNewWindowCreated
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