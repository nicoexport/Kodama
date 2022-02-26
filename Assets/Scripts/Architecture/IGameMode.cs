using UnityEngine;
using System;
using System.Collections;

public interface IGameMode
{
    GameModeState _state { get; }
    string _activeScene { get; }
    IEnumerator OnStart();
    IEnumerator OnEnd();
    void OnEditorStart();
}

public enum GameModeState
{
    Starting,
    Started,
    Ending,
    Ended,
    Loading
}