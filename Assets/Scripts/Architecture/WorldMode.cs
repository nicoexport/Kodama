using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WorldMode : IGameMode
{
    public GameModeState _state { get; private set; } = GameModeState.Ended;
    public string _activeScene { get; private set; }
    public WorldObject _currentWold { get; private set; }

    public IEnumerator OnStart()
    {
        if (_state != GameModeState.Ended) yield break;
        _state = GameModeState.Starting;
        // TO DO:Getting the last world the player has played in
        // Save = App.SaveService.GetSave(_saveID);
        // _currentWorld = Save.ActiveWorld 
        // for now testing with a Hard Coded World
        _currentWold = Resources.Load("World 0") as WorldObject;

        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));


        Debug.Log("OnWorldModeStart");
        _state = GameModeState.Started;
    }

    public IEnumerator OnEnd()
    {
        _state = GameModeState.Ending;
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Debug.Log("OnWorldModeEnd");
        _state = GameModeState.Ended;
    }

    public void OnEditorStart()
    {
        _currentWold = Resources.Load("World 0") as WorldObject;
        _activeScene = SceneManager.GetActiveScene().path;
        _state = GameModeState.Started;
    }
}