using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayMode : IGameMode
{
    public GameModeState _state { get; private set; } = GameModeState.Ended;
    public string _activeScene { get; private set; }
    private string _scenePath;

    public PlayMode(string scenePath)
    {
        this._scenePath = scenePath;
    }

    public IEnumerator OnStart()
    {
        if (_state != GameModeState.Ended) yield break;
        _state = GameModeState.Starting;

        // TO DO: LOAD SAVE

        _activeScene = _scenePath;

        yield return SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));
        Debug.Log("PLAY MODE STARTED");
        _state = GameModeState.Started;
    }

    public IEnumerator OnEnd()
    {
        _state = GameModeState.Ending;
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Debug.Log("PLAY MODE ENDED");
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