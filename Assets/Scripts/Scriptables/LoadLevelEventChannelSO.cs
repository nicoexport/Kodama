using UnityEngine;
using System;
/// <summary>
/// This class is used for scene loading events.
/// Takes a array of the scenes we want to load, a bool to specifiy if we want to unlod the currently active scene and a bool to specify if we want to show a screen fade in and out
///</summary>

[CreateAssetMenu(menuName = "Events/Load Level Event Channel")]
public class LoadLevelEventChannelSO : ScriptableObject
{
    public event Action<LevelDataSO, bool, bool> OnLoadingRequested;

    public void RaiseEvent(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
    {
        OnLoadingRequested?.Invoke(levelToLoad, unloadActiveScene, showScreenfade);
    }
}