using System;
using System.Collections.Generic;

namespace SaveLoad
{
    [Serializable]
    public class SaveData
    {
        public string Version;
        public LevelSaveData CurrentLevel;
        public WorldSaveData CurrentWorld;
        public List<WorldSaveData> WorldSaveDatas = new();
    }

    [Serializable]
    public class WorldSaveData
    {
        public string WorldName;
        public bool Unlocked;
        public bool Visited;
        public bool Completed;
        public List<LevelSaveData> LevelSaveDatas = new();
    }

    [Serializable]
    public class LevelSaveData
    {
        public string LevelName;
        public bool Unlocked;
        public bool Visited;
        public bool Completed;
        public float RecordTime;
    }
}