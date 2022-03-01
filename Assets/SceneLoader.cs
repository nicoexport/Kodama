using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private LoadLevelEventChannelSO _loadEventChannel;
    [SerializeField]
    private TransitionEventChannelSO _transitionEventChannel = default;


    private void OnEnable()
    {
        _loadEventChannel.OnLoadingLevelDataRequested += HandleLevelDataLoadingRequest;
        _loadEventChannel.OnLoadingScenePathRequested += HandleScenePathLoadingRequested;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnLoadingLevelDataRequested -= HandleLevelDataLoadingRequest;
        _loadEventChannel.OnLoadingScenePathRequested -= HandleScenePathLoadingRequested;
    }

    private void HandleLevelDataLoadingRequest(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {
        StartCoroutine(LoadScenes(levelToLoad.ScenePath, unloadActiveScene, showScreenfade));
    }

    private void HandleScenePathLoadingRequested(string sceneToLoad, bool unloadActiveScene, bool showScreenfade)
    {
        StartCoroutine(LoadScenes(sceneToLoad, unloadActiveScene, showScreenfade));
    }


    private IEnumerator LoadScenes(string scenePath, bool unloadActiveScene, bool showScreenfade)
    {


        if (showScreenfade)
        {
            _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 1f);
            yield return new WaitForSeconds(1f);
        }
        // yield return ScreenFade.Instance.Require(1f);

        if (unloadActiveScene)
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


        yield return SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(scenePath));


        if (showScreenfade)
        {
            _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, 1f);
            yield return new WaitForSeconds(1f);
        }
        // yield return ScreenFade.Instance.Release(1f);
    }
}
