using System.Collections;
using Scriptable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Architecture
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] LoadLevelEventChannelSO _loadEventChannel;

        [SerializeField] TransitionEventChannelSO _transitionEventChannel;


        void OnEnable()
        {
            _loadEventChannel.OnLoadingLevelDataRequested += HandleLevelDataLoadingRequest;
            _loadEventChannel.OnLoadingScenePathRequested += HandleScenePathLoadingRequested;
        }

        void OnDisable()
        {
            _loadEventChannel.OnLoadingLevelDataRequested -= HandleLevelDataLoadingRequest;
            _loadEventChannel.OnLoadingScenePathRequested -= HandleScenePathLoadingRequested;
        }

        void HandleLevelDataLoadingRequest(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
        {
            StartCoroutine(LoadScenes(levelToLoad.ScenePath, unloadActiveScene, showScreenfade));
        }

        void HandleScenePathLoadingRequested(string sceneToLoad, bool unloadActiveScene, bool showScreenfade)
        {
            StartCoroutine(LoadScenes(sceneToLoad, unloadActiveScene, showScreenfade));
        }


        IEnumerator LoadScenes(string scenePath, bool unloadActiveScene, bool showScreenfade)
        {
            if (showScreenfade)
            {
                _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 1f);
                yield return new WaitForSeconds(1f);
            }

            if (unloadActiveScene)
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


            yield return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePath));


            if (!showScreenfade) yield break;
            _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, 1f);
            //yield return new WaitForSeconds(1f);
        }
    }
}