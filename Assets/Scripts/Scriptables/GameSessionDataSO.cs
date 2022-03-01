using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Data/Game Session Data")]
public class GameSessionDataSO : ScriptableObject
{
    public List<WorldDataSO> WorldDatas;
    public LevelDataSO LevelSelect;

    public float TotalPlayTime { get; private set; }

    private float _currentSessionPlayTime = 0f;

    public WorldDataSO CurrentWorld;
    public LevelDataSO CurrentLevel;

}