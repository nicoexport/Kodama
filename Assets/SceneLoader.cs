using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private LoadLevelEventChannelSO _loadEventChannel;

    private void OnEnable()
    {
        _loadEventChannel.OnLoadingRequested += HandleLoadingRequest;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnLoadingRequested -= HandleLoadingRequest;
    }

    private void HandleLoadingRequest(LevelObject levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {
        StartCoroutine(LoadScenes(levelToLoad, unloadActiveScene, showScreenfade));
    }

    private IEnumerator LoadScenes(LevelObject levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {


        if (showScreenfade)
            yield return ScreenFade.Instance.Require(1f);

        if (unloadActiveScene)
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());


        yield return SceneManager.LoadSceneAsync(levelToLoad.ScenePath, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(levelToLoad.ScenePath));


        if (showScreenfade)
            yield return ScreenFade.Instance.Release(1f);
    }
}
