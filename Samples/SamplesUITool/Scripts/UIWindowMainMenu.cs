using SBN.UITool.Core.Elements;
using UnityEngine.SceneManagement;

public class UIWindowMainMenu : UIWindow 
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}