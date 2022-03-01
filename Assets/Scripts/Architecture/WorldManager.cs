using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [SerializeField]
    private WorldDataSO _defaultWorld;

    [SerializeField]
    private LoadLevelEventChannelSO _loadLevelEventChannelSO;

    private WorldDataSO _currentWorld;

    protected override void Awake()
    {
        base.Awake();
        _currentWorld = _defaultWorld;
    }

    private void StartLevel(LevelObject level)
    {
        GameModeManager.Instance.HandleLevelStartRequested(level);
    }

    [ContextMenu("StartLevel")]
    private void StartTestLevel()
    {
        // StartLevel(_currentWorld.LevelObjects[0]);
        var level = _currentWorld.LevelDatas[0];
        _loadLevelEventChannelSO.RaiseEventWithLevelData(level, true, true);
    }
}