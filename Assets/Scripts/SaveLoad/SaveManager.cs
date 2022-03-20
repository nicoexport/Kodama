using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveLoad
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField] private GameDataSO _gameData;
        [FormerlySerializedAs("_saveDataSo")] 
        [SerializeField] private SessionData _sessionData;
        
        public void OnSave()
        {
            var saveData = ConvertSessionData(_sessionData);
            if(SerializationManger.Save("test", saveData))
                print("SAVED THE GAME");
        }

        public void OnLoad()
        {
            ReadGameData(_gameData, _sessionData);
            
            var saveData = (SaveData)SerializationManger.Load(Application.persistentDataPath + "/saves/test.save");
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
        }

        public void OnDelete()
        {
            var path = Application.persistentDataPath + "/saves/test.save";
            File.Delete(path);
        }

        private SaveData ConvertSessionData(SessionData sessionData)
        {
            var currentWorldSaveData = ConvertWorldData(sessionData.CurrentWorld);
            var currentLevelSaveData = ConvertLevelData(sessionData.CurrentLevel);
            var saveData = new SaveData
            {
                Version = sessionData.Version,
                CurrentWorld = currentWorldSaveData,
                CurrentLevel = currentLevelSaveData
            };
            foreach (WorldData worldData in sessionData.WorldDatas)
            {
                var worldSaveData = ConvertWorldData(worldData);
                saveData.WorldSaveDatas.Add(worldSaveData);
            }
            
            return saveData;
        }
        
        private WorldSaveData ConvertWorldData(WorldData worldData)
        {
            var worldSaveData = new WorldSaveData
            {
                WorldName = worldData.WorldName,
                Unlocked = worldData.Unlocked,
                Visited = worldData.Visited,
                Completed = worldData.Completed
            };
            foreach (LevelData levelData in worldData.LevelDatas)
            {
                var levelSaveData = ConvertLevelData(levelData);
                worldSaveData.LevelSaveDatas.Add(levelSaveData);     
            }

            return worldSaveData;
        }
        
        private LevelSaveData ConvertLevelData(LevelData levelData)
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
        
        private void ReadGameData(GameDataSO gameDataSO, SessionData sessionData)
        {
            sessionData.Version = gameDataSO.Version;
            sessionData.MainMenuScenePath = gameDataSO.MainMenuScenePath;
            sessionData.WorldsScenePath = gameDataSO.WorldsScenePath;

            // reading World and level datas
            sessionData.WorldDatas.Clear();
            foreach (WorldDataSO worldSO in gameDataSO.WorldDatas)
            {
                var worldData = new WorldData(worldSO);

                foreach (LevelDataSO levelDataSO in worldSO.LevelDatas)
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
        
        private void ReadSaveDataIntoSessionData(SaveData saveData, SessionData sessionData)
        {
            string currentLevelName = saveData.CurrentLevel.LevelName;
            string currentWorldName = saveData.CurrentWorld.WorldName;
            for (var i = 0; i < sessionData.WorldDatas.Count; i++)
            {
                WorldData worldData = sessionData.WorldDatas[i];
                WorldSaveData worldSaveData = saveData.WorldSaveDatas[i];

                worldData.Unlocked = worldSaveData.Unlocked;
                worldData.Visited = worldSaveData.Visited;
                worldData.Completed = worldSaveData.Completed;
                if (worldData.WorldName == currentWorldName)
                    sessionData.CurrentWorld = worldData;

                for (var j = 0; j < worldData.LevelDatas.Count; j++)
                {
                    LevelData levelData = worldData.LevelDatas[j];
                    LevelSaveData levelSaveData = worldSaveData.LevelSaveDatas[j];

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
