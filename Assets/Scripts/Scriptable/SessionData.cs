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
    
    public void BreakInSaveData()
    {
        FreshSave = false;
    }

    /// <summary>
    /// Completes our session data by loading in save data,
    ///  such as the visited or completed state of worlds and levels
    /// </summary>
    void LoadSaveData()
    {

    }

    void SaveData()
    {

    }
}