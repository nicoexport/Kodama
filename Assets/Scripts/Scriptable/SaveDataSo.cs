using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(menuName = "Game Data/Game Session Data")]
public class SaveDataSo : ScriptableObject
{

    public string MainMenuScenePath;
    public string WorldsScenePath;
    public List<WorldData> WorldDatas = new List<WorldData>();
    public WorldData CurrentWorld;
    public LevelData CurrentLevel;
    public bool FreshSave { get; private set; } = true;


    /// <summary>
    /// Reads Data from a GameDataSO and sets up our session data by populating it.
    /// </summary>
    /// <param name="gameDataSO"></param>
    public void ReadGameData(GameDataSO gameDataSO)
    {
        FreshSave = true;
        WorldsScenePath = gameDataSO.WorldsScenePath;
        MainMenuScenePath = gameDataSO.MainMenuScenePath;

        // reading World and level datas
        WorldDatas.Clear();
        foreach (WorldDataSO worldSO in gameDataSO.WorldDatas)
        {
            var worldData = new WorldData(worldSO);

            foreach (LevelDataSO levelDataSO in worldSO.LevelDatas)
            {
                var levelData = new LevelData(levelDataSO);
                worldData.LevelDatas.Add(levelData);
            }
            WorldDatas.Add(worldData);
        }

        CurrentWorld = WorldDatas[0];
        CurrentLevel = CurrentWorld.LevelDatas[0];
        CurrentWorld.Unlocked = true;
    }

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