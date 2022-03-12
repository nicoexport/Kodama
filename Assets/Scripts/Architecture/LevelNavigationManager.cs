using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "CheckNamespace")]
public class LevelNavigationManager : MonoBehaviour
{
    [SerializeField] private GameSessionDataSO _sessionData;
    [SerializeField] private WorldDataSO _defaultWorld;
    [SerializeField] private LoadLevelEventChannelSO loadEvenChannel;

    private WorldData _selectedWorld;
    private LevelData _selectedLevel;

    public static event Action<WorldData, LevelData> OnWorldSelected;

    private void Awake()
    {
        _selectedWorld = _sessionData.CurrentWorld;
        Debug.Log(_selectedWorld);

        _selectedLevel = _selectedWorld.LevelDatas.Contains(_sessionData.CurrentLevel) ? _sessionData.CurrentLevel : _selectedWorld.LevelDatas[0];
        // if something went wrong and the selected world is still null, we will pick the default world assigned in inspector
        _selectedWorld ??= KodamaUtilities.GetWorldDataFromWorldDataSO(_defaultWorld, _sessionData);
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
        OnWorldSelected?.Invoke(_selectedWorld, _sessionData.CurrentLevel);
    }

    private void LoadLevel(LevelData obj)
    {
        loadEvenChannel.RaiseEventWithScenePath(obj.ScenePath,true, true);
    }
}
