using UnityEngine;
using System.Collections;

public class MainMenuMode : IGameMode
{
    public GameModeState _state { get; private set; } = GameModeState.Ended;
    public string _activeScene { get; private set; }

    public IEnumerator OnStart()
    {
        yield break;
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