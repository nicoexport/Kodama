using System.IO;
using Architecture;
using Data;
using Scriptable;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace SaveLoad
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField] GameDataSO _gameData;

        [FormerlySerializedAs("_saveDataSo")] [SerializeField]
        SessionData _sessionData;

        public void OnSave()
        {
            var saveData = ConvertSessionData(_sessionData);
            if (SerializationManger.Save("test", saveData))
                print("SAVED THE GAME");
        }

        public void OnLoad()
        {
            ReadGameData(_gameData, _sessionData);

            var saveData = (SaveData) SerializationManger.Load(Application.persistentDataPath + "/saves/test.save");
            if (saveData == null)
            {
                print("NO SAVE DATA FOUND: STARTING WITH NEW FILE");
                return;
            }

            if (saveData.Version != _sessionData.Version)
            {
                print("SAVE DATA VERSION MISMATCH: STARTING WITH NEW FILE");
                return;
            }

            ReadSaveDataIntoSessionData(saveData, _sessionData);
            print("SAVE FILE LOADED");
            print(_sessionData.CurrentWorld.WorldName + " " + _sessionData.CurrentLevel.LevelName);
        }

        public void OnDelete()
        {
            var path = Application.persistentDataPath + "/saves/test.save";
            File.Delete(path);
        }

        SaveData ConvertSessionData(SessionData sessionData)
        {
            var currentWorldSaveData = ConvertWorldData(sessionData.CurrentWorld);
            var currentLevelSaveData = ConvertLevelData(sessionData.CurrentLevel);
            var saveData = new SaveData
            {
                Version = sessionData.Version,
                CurrentWorld = currentWorldSaveData,
                CurrentLevel = currentLevelSaveData
            };
            foreach (var worldData in sessionData.WorldDatas)
            {
                var worldSaveData = ConvertWorldData(worldData);
                saveData.WorldSaveDatas.Add(worldSaveData);
            }

            print(saveData.CurrentWorld.WorldName + " " + saveData.CurrentLevel.LevelName);
            return saveData;
        }

        WorldSaveData ConvertWorldData(WorldData worldData)
        {
            var worldSaveData = new WorldSaveData
            {
                WorldName = worldData.WorldName,
                Unlocked = worldData.Unlocked,
                Visited = worldData.Visited,
                Completed = worldData.Completed
            };
            foreach (var levelData in worldData.LevelDatas)
            {
                var levelSaveData = ConvertLevelData(levelData);
                worldSaveData.LevelSaveDatas.Add(levelSaveData);
            }

            return worldSaveData;
        }

        LevelSaveData ConvertLevelData(LevelData levelData)
        {
            var levelSaveData = new LevelSaveData
            {
                LevelName = levelData.LevelName,
                Unlocked = levelData.Unlocked,
                Visited = levelData.Visited,
                Completed = levelData.Completed,
                RecordTime = levelData.RecordTime
            };
            return levelSaveData;
        }

        void ReadGameData(GameDataSO gameDataSO, SessionData sessionData)
        {
            sessionData.Version = gameDataSO.Version;
            sessionData.MainMenuScenePath = gameDataSO.MainMenuScenePath;
            sessionData.LevelSelectScenePath = gameDataSO.WorldsScenePath;

            // reading World and level datas
            sessionData.WorldDatas.Clear();
            foreach (var worldSO in gameDataSO.WorldDatas)
            {
                var worldData = new WorldData(worldSO);

                foreach (var levelDataSO in worldSO.LevelDatas)
                {
                    var levelData = new LevelData(levelDataSO);
                    worldData.LevelDatas.Add(levelData);
                }

                sessionData.WorldDatas.Add(worldData);
            }

            sessionData.CurrentWorld = sessionData.WorldDatas[0];
            sessionData.CurrentLevel = sessionData.CurrentWorld.LevelDatas[0];
            sessionData.CurrentWorld.Unlocked = true;
            sessionData.FreshSave = true;
        }

        void ReadSaveDataIntoSessionData(SaveData saveData, SessionData sessionData)
        {
            sessionData.FreshSave = false;
            var currentLevelName = saveData.CurrentLevel.LevelName;
            var currentWorldName = saveData.CurrentWorld.WorldName;
            for (var i = 0; i < sessionData.WorldDatas.Count; i++)
            {
                var worldData = sessionData.WorldDatas[i];
                var worldSaveData = saveData.WorldSaveDatas[i];

                worldData.Unlocked = worldSaveData.Unlocked;
                worldData.Visited = worldSaveData.Visited;
                worldData.Completed = worldSaveData.Completed;
                if (worldData.WorldName == currentWorldName)
                    sessionData.CurrentWorld = worldData;

                for (var j = 0; j < worldData.LevelDatas.Count; j++)
                {
                    var levelData = worldData.LevelDatas[j];
                    var levelSaveData = worldSaveData.LevelSaveDatas[j];

                    levelData.Unlocked = levelSaveData.Unlocked;
                    levelData.Visited = levelSaveData.Visited;
                    levelData.Completed = levelSaveData.Completed;
                    levelData.RecordTime = levelSaveData.RecordTime;
                    if (levelData.LevelName == currentLevelName)
                        sessionData.CurrentLevel = levelData;
                }
            }
        }
    }
}