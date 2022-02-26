using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    private WorldObject _currentWorld;

    protected override void Awake()
    {
        base.Awake();
        _currentWorld = GameModeManager.Instance.worldMode._currentWold;

        Debug.Log(_currentWorld.worldName);
    }

    private void StartLevel(LevelObject level)
    {
        GameModeManager.Instance.HandleLevelStartRequested(level);
    }

    [ContextMenu("StartLevel")]
    private void StartTestLevel()
    {
        StartLevel(_currentWorld.LevelObjects[0]);
    }
}