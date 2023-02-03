using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace SBN.SceneLoading
{
    /*
		TODO:
			- Load list of scenes (additively, obviously)
	 */

    public static class SceneLoader
    {
        /// <summary>
        /// Loads a new scene and sets it as "ActiveScene" while unloading all other scenes currently loaded.
        /// </summary>
        public static void LoadActiveScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
        }

        /// <summary>
        /// Loads a new scene asynchronously and sets it as "ActiveScene" while unloading all other scenes currently loaded.
        /// </summary>
        public static async UniTask LoadActiveSceneAsync(int buildIndex)
        {
            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
        }

        /// <summary>
        /// Unloads the currently active scene and loads the new scene with the given buildIndex
        /// </summary>
        public static async UniTask ChangeActiveSceneAsync(int buildIndex)
        {
            var sceneCount = SceneManager.sceneCount;

            if (sceneCount == 1)
            {
                await LoadActiveSceneAsync(buildIndex);
                return;
            }

            var previousScene = SceneManager.GetActiveScene();

            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            var newScene = SceneManager.GetSceneByBuildIndex(buildIndex);

            SceneManager.SetActiveScene(newScene);

            await SceneManager.UnloadSceneAsync(previousScene.buildIndex);
        }

        /// <summary>
        /// Loads an additive scene.
        /// </summary>
        public static void LoadSceneAdditive(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads an additive scene asynchronously.
        /// </summary>
        public static async UniTask LoadSceneAdditiveAsync(int buildIndex)
        {
            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloads a scene asynchronously
        /// </summary>
        public static async UniTask UnloadScene(int buildIndex)
        {
            await SceneManager.UnloadSceneAsync(buildIndex);
        }

        /// <summary>
        /// Loads a new scene and sets it as "ActiveScene" while unloading all other scenes currently loaded.
        /// </summary>
        public static void LoadActiveScene(string sceneName)
        {
            LoadActiveScene(GetSceneBuildIndexBySceneName(sceneName));
        }

        /// <summary>
        /// Loads a new scene asynchronously and sets it as "ActiveScene" while unloading all other scenes currently loaded.
        /// </summary>
        public static UniTask LoadActiveSceneAsync(string sceneName)
        {
            return LoadActiveSceneAsync(GetSceneBuildIndexBySceneName(sceneName));
        }

        /// <summary>
        /// Unloads the currently active scene and loads the new scene with the given buildIndex
        /// </summary>
        public static UniTask ChangeActiveSceneAsync(string sceneName)
        {
            return ChangeActiveSceneAsync(GetSceneBuildIndexBySceneName(sceneName));
        }

        /// <summary>
        /// Loads an additive scene.
        /// </summary>
        public static void LoadSceneAdditive(string sceneName)
        {
            LoadSceneAdditive(GetSceneBuildIndexBySceneName(sceneName));
        }

        /// <summary>
        /// Loads a scene additively.
        /// </summary>
        public static UniTask LoadSceneAdditiveAsync(string sceneName)
        {
            return LoadSceneAdditiveAsync(GetSceneBuildIndexBySceneName(sceneName));
        }

        /// <summary>
        /// Unloads a scene asynchronously
        /// </summary>
        public static UniTask UnloadSceneAsync(string sceneName)
        {
            return UnloadScene(GetSceneBuildIndexBySceneName(sceneName));
        }

        private static string GetSceneNameByIndex(int buildIndex)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            var slash = path.LastIndexOf('/');
            var name = path.Substring(slash + 1);
            var dot = name.LastIndexOf('.');

            return name.Substring(0, dot);
        }

        private static int GetSceneBuildIndexBySceneName(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var name = GetSceneNameByIndex(i);

                if (name == sceneName)
                    return i;
            }

            return -1;
        }
    }
}