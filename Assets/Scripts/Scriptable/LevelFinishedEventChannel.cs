using System;
using Data;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Events/ Level Finished Event Channel")]
    public class LevelFinishedEventChannel : ScriptableObject
    {
        public event Action<LevelData> OnLevelFinished;

        public void RaiseEvent(LevelData levelData)
        {
            OnLevelFinished?.Invoke(levelData);
        }
    }
}
