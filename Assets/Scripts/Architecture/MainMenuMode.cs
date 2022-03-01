using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuMode : IGameMode
{
    public GameModeState _state { get; private set; } = GameModeState.Ended;
    public string _activeScene { get; private set; }
    private string _scenePath;

    public MainMenuMode(string scenePath)
    {
        this._scenePath = scenePath;
    }

    public IEnumerator OnStart()
    {
        if (_state != GameModeState.Ended) yield break;
        _state = GameModeState.Starting;

        _activeScene = _scenePath;

        yield return SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));

        Debug.Log("MAIN MENU MODE STARTED");
        _state = GameModeState.Started;
    }

    public IEnumerator OnEnd()
    {
        _state = GameModeState.Ending;
        _state = GameModeState.Ended;
        yield break;
    }

    public void OnEditorStart()
    {

    }
}