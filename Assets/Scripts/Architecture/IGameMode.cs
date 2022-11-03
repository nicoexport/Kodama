using System.Collections;

namespace Kodama.Architecture {
    public interface IGameMode {
        GameModeState _state { get; }
        string _activeScene { get; }
        IEnumerator OnStart();
        IEnumerator OnEnd();
        void OnEditorStart();
    }

    public enum GameModeState {
        Starting,
        Started,
        Ending,
        Ended,
        Loading
    }
}