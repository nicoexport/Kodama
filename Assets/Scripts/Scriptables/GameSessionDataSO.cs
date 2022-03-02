using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Data/Game Session Data")]
public class GameSessionDataSO : ScriptableObject
{

    public string MainMenuScenePath;
    public string WorldsScenePath;
    public List<WorldData> WorldDatas = new List<WorldData>();
    public WorldData CurrentWorld;
    public LevelData CurrentLevel;


    /// <summary>
    /// Reads Data from a GameDataSO and sets up our session data by populating it.
    /// </summary>
    /// <param name="gameDataSO"></param>
    public void ReadGameData(GameDataSO gameDataSO)
    {
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
    }


    /// <summary>
    /// Completes our session data by loading in save data,
    ///  such as the visited or completed state of worlds and levels
    /// </summary>
    void LoadSaveData()
    {

    }

    void SaveSessionData()
    {

    }
}

public class LevelData
{
    // loaded from game data
    public string ScenePath;
    public string LevelName;
    public Sprite LevelImage;
    // loaded from save data
    public bool Visited;
    public bool Completed;
    public float RecordTime;

    public LevelData(LevelDataSO levelDataSO)
    {
        this.ScenePath = levelDataSO.ScenePath;
        this.LevelName = levelDataSO.LevelName;
        this.LevelImage = levelDataSO.LevelImage;
    }
}

public class WorldData
{

    // loaded from game data
    public string WorldName;
    // loaded from save data
    public bool Visited;
    public bool Completed;

    public List<LevelData> LevelDatas = new List<LevelData>();

    public WorldData(WorldDataSO worldDataSO)
    {
        this.WorldName = worldDataSO.WorldName;
    }
}