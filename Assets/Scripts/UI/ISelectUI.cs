using System.Collections;
using UnityEngine;
    
public interface ISelectUI
{
    IEnumerator OnStart(SessionData sessionData);
    IEnumerator OnEnd();

    SelectUIState _state { get; }
}

public enum SelectUIState
{
    Starting,
    Started,
    Ending,
    Ended
}
