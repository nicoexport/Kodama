using System;
using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Kodama.Scriptable {
    [Serializable]
    [CreateAssetMenu(menuName = "Game Data/World Data")]
    public class WorldDataSO : ScriptableObject {
        public string WorldName;
        [Expandable]
        public List<LevelDataSO> LevelDatas;
        [Expandable]
        public WorldStyleSo WorldStyleSo;
    }
}