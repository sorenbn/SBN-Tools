using SBN.SceneHelper.Interfaces;
using SBN.Utilities.ExtensionMethods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SBN.SceneHelper.Examples
{
    public class UILevel : MonoBehaviour
    {
        [SerializeField]
        private Text sceneNameField;

        [SerializeField]
        private Text sceneStatusField;

        [SerializeField]
        private Button changeLevelButton;

        [SerializeField]
        private int levelChangeIndex = 0;

        private void Start()
        {
            sceneNameField.text = SceneManager.GetActiveScene().name;
        }

        private void OnEnable()
        {
            var sceneObservable = this.GetScriptOfType<ISceneObservable>();

            if (sceneObservable is null)
                return;

            sceneObservable.OnSceneIntialize += SceneObservable_OnSceneIntialize;
            sceneObservable.OnSceneReady += SceneObservable_OnSceneReady;

            changeLevelButton.interactable = false;
            changeLevelButton.onClick.AddListener(OnChangeLevelButtonClick);
        }

        private void OnChangeLevelButtonClick()
        {
            SceneManager.LoadScene(levelChangeIndex, LoadSceneMode.Single);
        }

        private void OnDisable()
        {
            var sceneObservable = this.GetScriptOfType<ISceneObservable>();

            if (sceneObservable is null)
                return;

            sceneObservable.OnSceneIntialize -= SceneObservable_OnSceneIntialize;
            sceneObservable.OnSceneReady -= SceneObservable_OnSceneReady;
        }

        private void SceneObservable_OnSceneIntialize(Scene obj)
        {
            sceneStatusField.color = Color.yellow;
            sceneStatusField.text = "Status: loading scene...";
        }

        private void SceneObservable_OnSceneReady(Scene obj)
        {
            sceneStatusField.color = Color.green;
            sceneStatusField.text = "Status: scene ready!";

            changeLevelButton.interactable = true;
        }
    }
}