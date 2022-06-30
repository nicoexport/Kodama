using System;
using Scriptable;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LevelData
    {
        public bool Visited;
        public bool Completed;
        public float RecordTime = Mathf.Infinity;
        public readonly AudioCueSo LevelMusicAudioCueSo;
        public readonly string LevelName;
        public readonly string ScenePath;
        Sprite LevelImage;


        public LevelData(LevelDataSO levelDataSO)
        {
            ScenePath = levelDataSO.ScenePath;
            LevelName = levelDataSO.LevelName;
            LevelImage = levelDataSO.LevelImage;
            LevelMusicAudioCueSo = levelDataSO.LevelMusicAudioCueSo;
        }

        public bool Unlocked { get; set; } = false;

        public bool UpdateRecordTime(float time)
        {
            if (!(time < RecordTime)) return false;
            RecordTime = time;
            return true;
        }
    }
}