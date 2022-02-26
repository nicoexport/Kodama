using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelMode : IGameMode
{
    public GameModeState _state { get; private set; } = GameModeState.Ended;
    public string _activeScene { get; private set; }

    public IEnumerator OnStart()
    {
        if (_state != GameModeState.Ended) yield break;
        _state = GameModeState.Starting;

        _activeScene = GameModeManager.Instance._levelToLoad;

        yield return SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));
        Debug.Log("OnLevelModeStart");
        _state = GameModeState.Started;
    }

    public IEnumerator OnEnd()
    {
        _state = GameModeState.Ending;
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Debug.Log("OnLevelModeEnd");
        _state = GameModeState.Ended;
    }

    public void OnEditorStart()
    {
#if UNITY_EDITOR
        _activeScene = SceneManager.GetActiveScene().path;
        _state = GameModeState.Started;
#endif
    }
}