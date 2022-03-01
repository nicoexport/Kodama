using UnityEngine;
using System;
/// <summary>
/// This class is used for scene loading events.
/// Takes a array of the scenes we want to load, a bool to specifiy if we want to unlod the currently active scene and a bool to specify if we want to show a screen fade in and out
///</summary>

[CreateAssetMenu(menuName = "Events/Load Level Event Channel")]
public class LoadLevelEventChannelSO : ScriptableObject
{
    public event Action<LevelDataSO, bool, bool> OnLoadingLevelDataRequested;
    public event Action<string, bool, bool> OnLoadingScenePathRequested;

    public void RaiseEventWithLevelData(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {
        OnLoadingLevelDataRequested?.Invoke(levelToLoad, unloadActiveScene, showScreenfade);
    }

    public void RaiseEventWithScenePath(string scenePath, bool unloadActiveScene, bool showScreenfade)
    {
        OnLoadingScenePathRequested?.Invoke(scenePath, unloadActiveScene, showScreenfade);
    }

}