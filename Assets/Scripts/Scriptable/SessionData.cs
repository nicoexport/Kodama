using System;
using System.Collections.Generic;
using Kodama.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kodama.Scriptable {
    [Serializable]
    [CreateAssetMenu(menuName = "Game Data/Game Session Data")]
    public class SessionData : ScriptableObject {
        public string Version;
        public string MainMenuScenePath;

        [FormerlySerializedAs("WorldsScenePath")]
        public string LevelSelectScenePath;

        public List<WorldData> WorldDatas = new();
        public WorldData CurrentWorld;
        public LevelData CurrentLevel;
        public bool FreshSave = true;
    }
}