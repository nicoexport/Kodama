using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldsScreenManager : Singleton<WorldsScreenManager>
{
    [SerializeField]
    private GameSessionDataSO _sessionData;
    [SerializeField]
    private LoadLevelEventChannelSO _loadLevelEventChannel;


    [Header("Temporary")]
    [SerializeField] TextMeshProUGUI worldDisplay;

    private WorldData _currentWorld;

    protected override void Awake()
    {
        base.Awake();
        _currentWorld = _sessionData.CurrentWorld;
        UpdateWorldDisplay();
        StartTestLevel();
    }


    [ContextMenu("StartLevel")]
    private void StartTestLevel()
    {
        var level = _currentWorld.LevelDatas[0];
        _loadLevelEventChannel.RaiseEventWithScenePath(level.ScenePath, true, true);
    }


    private void UpdateWorldDisplay()
    {
        worldDisplay.text = _currentWorld.WorldName;
    }

    [ContextMenu("ReturnToMainMenu")]
    private void ReturnToMainMenu()
    {
        GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
    }
}