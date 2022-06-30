using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Game Data/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [FormerlySerializedAs("_scenePath")] public string ScenePath;

        public string LevelName;
        public Sprite LevelImage;
        public AudioCueSo LevelMusicAudioCueSo;
        public bool Visited;
        public bool Completed;

        public float RecordTime = Mathf.Infinity;

        public void UpdateRecordTime(float time)
        {
            if (time < RecordTime)
                RecordTime = time;
        }
    }
}