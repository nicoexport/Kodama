using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Game Data/Game Session Data")]
    public class SessionData : ScriptableObject
    {
        public string Version;
        public string MainMenuScenePath;
        [FormerlySerializedAs("WorldsScenePath")] 
        public string LevelSelectScenePath;
        public List<WorldData> WorldDatas = new List<WorldData>();
        public WorldData CurrentWorld;
        public LevelData CurrentLevel;
        public bool FreshSave = true;
    
    }
}