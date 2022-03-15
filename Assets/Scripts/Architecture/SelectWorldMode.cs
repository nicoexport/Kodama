using System.Collections;

public class SelectWorldMode : ILevelSelectMode
{
    public LevelSelectModeState _state { get; } = LevelSelectModeState.Ended;
   
    public IEnumerator OnStart()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator OnEnd()
    {
        throw new System.NotImplementedException();
    }
}