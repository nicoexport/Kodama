using Data;
using Scriptable;
using UnityEngine;

namespace Architecture
{
    public class LevelFlowHandler : MonoBehaviour
    {
        [SerializeField] SessionData _sessionData;

        [SerializeField] LoadLevelEventChannelSO _loadLevelEventChannel;

        [SerializeField] VoidEventChannelSO _returnToWorldScreenEvent;

        [SerializeField] VoidEventChannelSO _returnToMainMenuEvent;


        void OnEnable()
        {
            _returnToMainMenuEvent.OnEventRaised += LoadMainMenu;
            _returnToWorldScreenEvent.OnEventRaised += LoadWorldScreen;
        }


        void OnDisable()
        {
            _returnToMainMenuEvent.OnEventRaised -= LoadMainMenu;
            _returnToWorldScreenEvent.OnEventRaised -= LoadWorldScreen;
        }

        // Takes in a LevelDataSO. If its not the Worlds last level it loads the next level of the same world, if it is it loads the next world. 
        public void NextLevelRequest(LevelData levelData)
        {
            Debug.Log("LEVELFLOW: TRYING NEXT LEVEL REQUEST");
            Debug.Log("Level Name: " + levelData.LevelName);
            // iterate over all worlds
            for (var i = 0; i < _sessionData.WorldDatas.Count; i++)
                // Iterate over the worlds levels 
            for (var j = 0; j < _sessionData.WorldDatas[i].LevelDatas.Count; j++)
                if (_sessionData.WorldDatas[i].LevelDatas[j].LevelName == levelData.LevelName)
                {
                    // if its not the last level of a world just load the next level of the world.
                    if (j < _sessionData.WorldDatas[i].LevelDatas.Count - 1)
                    {
                        LoadNextLevel(_sessionData.WorldDatas[i].LevelDatas[j + 1]);
                        return;
                    }

                    // if its the last level of the world and its not the last world load the next world.

                    if (i < _sessionData.WorldDatas.Count - 1)
                    {
                        LoadNextWorld(_sessionData.WorldDatas[i + 1]);
                        return;
                    }

                    // if its the last level of the last world load the credits.

                    LoadCredits();
                    return;
                }
        }

        void LoadNextLevel(LevelData levelData)
        {
            _sessionData.CurrentLevel = levelData;
            _loadLevelEventChannel.RaiseEventWithScenePath(levelData.ScenePath, true, true);
        }

        void LoadNextWorld(WorldData worldData)
        {
            _sessionData.CurrentWorld = worldData;
            _sessionData.CurrentLevel = worldData.LevelDatas[0];
            _loadLevelEventChannel.RaiseEventWithScenePath(_sessionData.LevelSelectScenePath, true, true);
            Debug.Log("TO DO: Load Next World");
        }

        public void ExitLevelEarly(LevelData currentLevel)
        {
            _sessionData.CurrentLevel = currentLevel;
            _loadLevelEventChannel.RaiseEventWithScenePath(_sessionData.LevelSelectScenePath, true, true);
        }

        public void FinishLevelAndExit(LevelData currentLevel)
        {
            // var nextLevel = Utilities.GameSessionGetNextLevelData(currentLevel, _sessionData);
            // if (_sessionData.CurrentWorld.LevelDatas.Contains(nextLevel))
            //     _sessionData.CurrentLevel = nextLevel;
            _loadLevelEventChannel.RaiseEventWithScenePath(_sessionData.LevelSelectScenePath, true, true);
        }

        void LoadWorldScreen()
        {
            _loadLevelEventChannel.RaiseEventWithScenePath(_sessionData.LevelSelectScenePath, true, true);
        }

        void LoadMainMenu()
        {
            GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
        }

        void LoadCredits()
        {
            Debug.Log("TO DO: Load Credits");
            LoadMainMenu();
        }
    }
}