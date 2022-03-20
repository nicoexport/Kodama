using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(menuName = "Game Data/Game Session Data")]
public class SessionData : ScriptableObject
{
    public string Version;
    public string MainMenuScenePath;
    public string WorldsScenePath;
    public List<WorldData> WorldDatas = new List<WorldData>();
    public WorldData CurrentWorld;
    public LevelData CurrentLevel;
    public bool FreshSave = true;
    
}