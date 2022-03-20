using UnityEngine;
using UnityEngine.Serialization;

namespace SaveLoad
{
    public class SaveManager : Singleton<SaveManager>
    {
        [FormerlySerializedAs("_saveDataSo")] 
        [SerializeField] private SessionData _sessionData;
        
        public void SaveSessionData()
        {
            var saveData = ConvertSessionData(_sessionData);
            if(SerializationManger.Save("test", saveData))
                print("SAVED THE GAME");
        }

        public bool OnLoad()
        {
            return false;
        }

        private SaveData ConvertSessionData(SessionData sessionData)
        {
            var currentWorldSaveData = ConvertWorldData(sessionData.CurrentWorld);
            var currentLevelSaveData = ConvertLevelData(sessionData.CurrentLevel);
            var saveData = new SaveData
            {
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
    }
}
