using System.Collections.Generic;
using Scriptable;

namespace Data
{
    [System.Serializable]
    public class WorldData
    {
        public string WorldName;
        private bool _unlocked;
        public bool Visited;
        public bool Completed;
        public WorldStyleSo Style;

        public bool Unlocked
        {
            get => _unlocked;
            set
            {
                _unlocked = value;
                if (value == true)
                    LevelDatas[0].Unlocked = true;
            }
        }
    
        public List<LevelData> LevelDatas = new List<LevelData>();

        public WorldData(WorldDataSO worldDataSo)
        {
            this.WorldName = worldDataSo.WorldName;
            this.Style = worldDataSo.WorldStyleSo;
        }
    
    
    }
}