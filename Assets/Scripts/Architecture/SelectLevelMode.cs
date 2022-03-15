using System;
using System.Collections;

public class SelectLevelMode : ILevelSelectMode
{
    public SelectLevelMode(LevelSelectManager manager, LevelSelectionUI ui, GameSessionDataSO sessionData)
    {
        this._ui = ui;
        this._manager = manager;
        this._sessionData = sessionData;
        LevelNavigationSocket.OnButtonClickedAction += LoadLevel; 
    }
    
    public LevelSelectModeState _state { get; private set; } = LevelSelectModeState.Ended;

    private LevelSelectManager _manager;
    private LevelSelectionUI _ui;
    private GameSessionDataSO _sessionData;

    public IEnumerator OnStart()
    {
        if (_state != LevelSelectModeState.Ended) yield break;
        _state = LevelSelectModeState.Starting;
        
        _ui.gameObject.SetActive(true);
        yield return _ui.SetupUI(_sessionData.CurrentWorld, _sessionData.CurrentLevel);
        _state = LevelSelectModeState.Started;
    }

    public IEnumerator OnEnd()
    {
        _state = LevelSelectModeState.Ending;
        LevelNavigationSocket.OnButtonClickedAction -= LoadLevel;
        
        _ui.gameObject.SetActive(false);
        _state = LevelSelectModeState.Ended;
        yield break;
    }   

    private void LoadLevel(LevelData obj)
    {
        _manager.LoadLevel(obj);
    }
}