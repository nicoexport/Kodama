using UnityEngine;
using System;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameSessionDataSO _sessionData;
    [SerializeField] private WorldDataSO _defaultWorld;

    private WorldData _selectedWorld;
    private LevelData _selectedLevel;

    public static event Action<WorldData> OnWorldSelected;

    private void Awake()
    {
        _selectedWorld = _sessionData.CurrentWorld;
        Debug.Log(_selectedWorld);

        if (_selectedWorld.LevelDatas.Contains(_sessionData.CurrentLevel))
            _selectedLevel = _sessionData.CurrentLevel;
        else
            _selectedLevel = _selectedWorld.LevelDatas[0];


        // if something went wrong and the selected world is still null, we will pick the default world assigned in inspector
        if (_selectedWorld == null)
            _selectedWorld = KodamaUtilities.GetWorldDataFromWorldDataSO(_defaultWorld, _sessionData);
    }

    private void Start()
    {
        OnWorldSelected?.Invoke(_selectedWorld);
    }
}