using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Serialization;

[SuppressMessage("ReSharper", "CheckNamespace")]
public class LevelNavigationManager : MonoBehaviour
{
    public static event Action<WorldData, LevelData> OnLevelSelectStart;
    public static event Action OnLevelSelectEnd;

    public static event Action<GameSessionDataSO> OnWorldSelectStart;
    public static event Action OnWorldSelectEnd;
    
    [SerializeField] private GameSessionDataSO _sessionData;
    [SerializeField] private WorldDataSO _defaultWorld;
    [FormerlySerializedAs("loadEvenChannel")] [SerializeField] private LoadLevelEventChannelSO _loadEvenChannel;
    [SerializeField] private TransitionEventChannelSO _transitionEventChannel;
    [SerializeField] private float _transitionDurationInSeconds;

    [SerializeField] private LevelNavigationUI _levelSelect;
    [SerializeField] private WorldSelect _worldSelect;

    private WorldData _selectedWorld;
    private LevelData _selectedLevel;

    private bool _isSwitching;
    private WaitForSeconds _waitForTransition;
    private ISelectUI _currentUI;
    private void Awake()
    {
        _waitForTransition = new WaitForSeconds(_transitionDurationInSeconds);
        
        _selectedWorld = _sessionData.CurrentWorld;
        _selectedLevel = _selectedWorld.LevelDatas.Contains(_sessionData.CurrentLevel) ? _sessionData.CurrentLevel : _selectedWorld.LevelDatas[0];
        _selectedWorld ??= KodamaUtilities.GetWorldDataFromWorldDataSO(_defaultWorld, _sessionData);
    }

    private void OnEnable()
    {
        LevelSelectSocket.OnButtonClickedAction += LoadLevel;
    }

    private void OnDisable()
    {
        LevelSelectSocket.OnButtonClickedAction -= LoadLevel;
    }

    private void Start()
    {
        SwitchUI(_levelSelect);
    }

    private void SwitchUI(ISelectUI selectUI)
    {
        StartCoroutine(SwitchUIEnumerator(selectUI));
    }

    private IEnumerator SwitchUIEnumerator(ISelectUI selectUI)
    {
        yield return new WaitUntil(() => !_isSwitching);
        if (_currentUI == selectUI) yield break;
        

        if (_currentUI != null)
        {
            _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 0.5f);
            yield return _waitForTransition;
            yield return _currentUI.OnEnd();
        }

        _currentUI = selectUI;

        yield return _currentUI.OnStart(_sessionData);
        
        _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, 0.5f);
        yield return _waitForTransition;

        _isSwitching = false;
    }

    private void LoadLevel(LevelData obj)
    {
        _loadEvenChannel.RaiseEventWithScenePath(obj.ScenePath,true, true);
    }
    
    [ContextMenu("UnlockLevels")]
    private void TestUnlockLevelsOfCurrentWorld()
    {
        foreach (var level in _sessionData.CurrentWorld.LevelDatas)
        {
            level.Unlocked = true;
        }

        StartCoroutine(_levelSelect.OnStart(_sessionData));
    }
}

public enum LevelSelectionState
{
    Level,
    World
}
    
