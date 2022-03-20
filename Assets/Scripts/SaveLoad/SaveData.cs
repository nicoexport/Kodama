using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    [System.Serializable]
    public class SaveData
    {
        public LevelSaveData CurrentLevel;
        public WorldSaveData CurrentWorld;
        public List<WorldSaveData> WorldSaveDatas = new List<WorldSaveData>();
        
    }
    
    [System.Serializable]
    public class LevelSaveData
    {
        public string LevelName;
        public bool Unlocked;
        public bool Visited;
        public bool Completed;
        public float RecordTime;
    }

    [System.Serializable]
    public class WorldSaveData
    {
        public string WorldName;
        public bool Unlocked;
        public bool Visited;
        public bool Completed;
        public List<LevelSaveData> LevelSaveDatas = new List<LevelSaveData>();
    }
}