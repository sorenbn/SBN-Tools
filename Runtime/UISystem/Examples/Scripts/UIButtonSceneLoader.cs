using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonSceneLoader : MonoBehaviour 
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}