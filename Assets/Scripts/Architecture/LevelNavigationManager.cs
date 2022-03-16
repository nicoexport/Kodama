using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Serialization;

[SuppressMessage("ReSharper", "CheckNamespace")]
public class LevelNavigationManager : MonoBehaviour
{
    [SerializeField] private GameSessionDataSO _sessionData;
    [SerializeField] private WorldDataSO _defaultWorld;
    [FormerlySerializedAs("loadEvenChannel")] [SerializeField] private LoadLevelEventChannelSO _loadEvenChannel;
    [SerializeField] private TransitionEventChannelSO _transitionEventChannel;

    private WorldData _selectedWorld;
    private LevelData _selectedLevel;
    private LevelSelectionState _selectionState;

    public static event Action<WorldData, LevelData> OnLevelSelectStart;
    public static event Action OnLevelSelectEnd;

    public static event Action<WorldData> OnWorldModeStart;
    public static event Action OnWorldModeEnd;

    private void Awake()
    {
        _selectedWorld = _sessionData.CurrentWorld;
        _selectedLevel = _selectedWorld.LevelDatas.Contains(_sessionData.CurrentLevel) ? _sessionData.CurrentLevel : _selectedWorld.LevelDatas[0];
        _selectedWorld ??= KodamaUtilities.GetWorldDataFromWorldDataSO(_defaultWorld, _sessionData);

        _selectionState = LevelSelectionState.Level;    
    }

    private void OnEnable()
    {
        LevelNavigationSocket.OnButtonClickedAction += LoadLevel;
    }

    private void OnDisable()
    {
        LevelNavigationSocket.OnButtonClickedAction -= LoadLevel;
    }

    private void Start()
    {
        if(_selectionState == LevelSelectionState.Level)
            OnLevelSelectStart?.Invoke(_selectedWorld, _sessionData.CurrentLevel);
    }

    private void LoadLevel(LevelData obj)
    {
        _loadEvenChannel.RaiseEventWithScenePath(obj.ScenePath,true, true);
    }

    private void SwitchToLevelMode()
    {
        _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 0.5f);

    }

    private void SwitchToWorldMode()
    {
        
    }

    [ContextMenu("UnlockLevels")]
    private void TestUnlockLevelsOfCurrentWorld()
    {
        foreach (var level in _selectedWorld.LevelDatas)
        {
            level.Unlocked = true;
        }
        OnLevelSelectStart?.Invoke(_selectedWorld , _sessionData.CurrentLevel);
    }
}

public enum LevelSelectionState
{
    Level,
    World
}
    
