using System;
using System.Collections;
using UnityEngine;

namespace Architecture
{
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
}