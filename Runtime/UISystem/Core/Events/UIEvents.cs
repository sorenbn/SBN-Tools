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

public struct UIEventWindowShow
{
    public UIWindowAsset WindowAsset
    {
        get; set;
    }
}

public struct UIEventWindowHide
{
    public UIWindowAsset WindowAsset
    {
        get; set;
    }
}