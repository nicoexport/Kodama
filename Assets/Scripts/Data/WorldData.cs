using System;
using System.Collections.Generic;
using Scriptable;

namespace Data {
    [Serializable]
    public class WorldData {
        public string WorldName;
        public bool Visited;
        public bool Completed;
        public WorldStyleSo Style;

        public List<LevelData> LevelDatas = new();
        private bool _unlocked;

        public WorldData(WorldDataSO worldDataSo) {
            WorldName = worldDataSo.WorldName;
            Style = worldDataSo.WorldStyleSo;
        }

        public bool Unlocked {
            get => _unlocked;
            set {
                _unlocked = value;
                if (value) {
                    LevelDatas[0].Unlocked = true;
                }
            }
        }
    }
}