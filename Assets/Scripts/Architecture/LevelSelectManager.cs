using UnityEngine;
using System;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] GameSessionDataSO _sessionData;

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
    }

    private void Start()
    {
        OnWorldSelected?.Invoke(_selectedWorld);
    }
}