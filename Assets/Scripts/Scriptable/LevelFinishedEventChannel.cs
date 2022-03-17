using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/ Level Finished Event Channel")]
public class LevelFinishedEventChannel : ScriptableObject
{
    public event Action<LevelData> OnLevelFinished;

    public void RaiseEvent(LevelData levelData)
    {
        OnLevelFinished?.Invoke(levelData);
    }
}
