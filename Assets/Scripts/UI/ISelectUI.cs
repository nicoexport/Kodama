using System.Collections;
using Kodama.Scriptable;

namespace Kodama.UI {
    public interface ISelectUI {
        SelectUIState _state { get; }
        IEnumerator OnStart(SessionData sessionData);
        IEnumerator OnEnd();
    }

    public enum SelectUIState {
        Starting,
        Started,
        Ending,
        Ended
    }
}