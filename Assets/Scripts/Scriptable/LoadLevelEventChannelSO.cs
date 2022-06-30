using UnityEngine;

namespace Scriptable
{
    /// <summary>
    ///     This class is used for scene loading events.
    ///     Takes a array of the scenes we want to load, a bool to specifiy if we want to unlod the currently active scene and
    ///     a bool to specify if we want to show a screen fade in and out
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Load Level Event Channel")]
    public class LoadLevelEventChannelSO : ScriptableObject
    {
        public delegate void LoadLevelWithData(LevelDataSO levelDataSo, bool unloadActiveScene, bool showScreenfade);

        public delegate void LoadLevelWithPath(string path, bool unloadActiveScene, bool showScreenfade);

        public event LoadLevelWithData OnLoadingLevelDataRequested;

        public event LoadLevelWithPath OnLoadingScenePathRequested;

        public void RaiseEventWithLevelDataSO(LevelDataSO levelToLoad, bool unloadActiveScene, bool showScreenfade)
        {
            OnLoadingLevelDataRequested?.Invoke(levelToLoad, unloadActiveScene, showScreenfade);
        }

        public void RaiseEventWithScenePath(string scenePath, bool unloadActiveScene, bool showScreenfade)
        {
            OnLoadingScenePathRequested?.Invoke(scenePath, unloadActiveScene, showScreenfade);
        }
    }
}