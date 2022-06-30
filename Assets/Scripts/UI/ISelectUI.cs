using System.Collections;
using Scriptable;

namespace UI
{
    public interface ISelectUI
    {
        SelectUIState _state { get; }
        IEnumerator OnStart(SessionData sessionData);
        IEnumerator OnEnd();
    }

    public enum SelectUIState
    {
        Starting,
        Started,
        Ending,
        Ended
    }
}