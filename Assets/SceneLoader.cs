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
        _loadEventChannel.OnLoadingRequested += HandleLoadingRequest;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnLoadingRequested -= HandleLoadingRequest;
    }

    private void HandleLoadingRequest(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {
        StartCoroutine(LoadScenes(levelToLoad, unloadActiveScene, showScreenfade));
    }

    private IEnumerator LoadScenes(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {


        if (showScreenfade)
        {
            _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 1f);
            yield return new WaitForSeconds(1f);
        }
        // yield return ScreenFade.Instance.Require(1f);

        if (unloadActiveScene)
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


        yield return SceneManager.LoadSceneAsync(levelToLoad.ScenePath, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(levelToLoad.ScenePath));


        if (showScreenfade)
        {
            _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, 1f);
            yield return new WaitForSeconds(1f);
        }
        // yield return ScreenFade.Instance.Release(1f);
    }
}
