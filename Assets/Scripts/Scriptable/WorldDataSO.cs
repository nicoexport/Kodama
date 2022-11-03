using System.Collections.Generic;
using UnityEngine;

namespace Kodama.Scriptable {
    [CreateAssetMenu(menuName = "Game Data/World Data")]
    public class WorldDataSO : ScriptableObject {
        public string WorldName;
        public List<LevelDataSO> LevelDatas;
        public WorldStyleSo WorldStyleSo;
    }
}