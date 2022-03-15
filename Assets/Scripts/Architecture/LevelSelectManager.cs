using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Serialization;

[SuppressMessage("ReSharper", "CheckNamespace")]
public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameSessionDataSO _sessionData;
    [SerializeField] private WorldDataSO _defaultWorld;
    [FormerlySerializedAs("loadEvenChannel")] [SerializeField] private LoadLevelEventChannelSO _loadEvenChannel;
    [SerializeField] private LevelSelectionUI _levelSelectionUI;
    [SerializeField] private TransitionEventChannelSO _transitionEventChannel;
    [SerializeField] private float _transitionDurationInSeconds;
    
    private WorldData _selectedWorld;
    private LevelData _selectedLevel;

    private ILevelSelectMode _currentMode;
    private SelectLevelMode _levelSelect;
    private SelectWorldMode _worldSelect;

    private bool _isSwitching = false; 

    public static event Action<WorldData, LevelData> OnWorldSelected;

    private void Awake()
    {
        _levelSelect = new SelectLevelMode(this, _levelSelectionUI, _sessionData);
        _worldSelect = new SelectWorldMode();
        
        _selectedWorld = _sessionData.CurrentWorld;
        _selectedLevel = _selectedWorld.LevelDatas.Contains(_sessionData.CurrentLevel) ? _sessionData.CurrentLevel : _selectedWorld.LevelDatas[0];
        _selectedWorld ??= KodamaUtilities.GetWorldDataFromWorldDataSO(_defaultWorld, _sessionData);
        
        SwitchMode(_levelSelect);
    }

    private void SwitchMode(ILevelSelectMode mode)
    {
        StartCoroutine(SwitchModeEnumerator(mode));
    }
    
    private IEnumerator SwitchModeEnumerator(ILevelSelectMode mode)
    {
        yield return new WaitUntil(() => !_isSwitching);
        if (_currentMode == mode) yield break;

        _isSwitching = true;
        
        _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, _transitionDurationInSeconds);
        yield return new WaitForSeconds(_transitionDurationInSeconds);

        if (_currentMode != null)
            yield return _currentMode.OnEnd();

        _currentMode = mode;
        yield return _currentMode.OnStart();
        
        _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, _transitionDurationInSeconds);
        yield return new WaitForSeconds(_transitionDurationInSeconds);

        _isSwitching = false;
    }

    private void OnEnable()
    {
       // LevelNavigationSocket.OnButtonClickedAction += LoadLevel;
    }

    private void OnDisable()
    {
        // LevelNavigationSocket.OnButtonClickedAction -= LoadLevel;
    }

    private void Start()
    {
        OnWorldSelected?.Invoke(_selectedWorld, _sessionData.CurrentLevel);
    }

    public void LoadLevel(LevelData obj)
    {
        _loadEvenChannel.RaiseEventWithScenePath(obj.ScenePath,true, true);
    }
}

