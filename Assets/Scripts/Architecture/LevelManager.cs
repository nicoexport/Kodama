using System;
using Data;
using GameManagement;
using Level.Logic;
using Scriptable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utility;

namespace Architecture
{
    [RequireComponent(typeof(LevelFlowHandler))]
    [RequireComponent(typeof(LevelTimer))]
    public class LevelManager : Singleton<LevelManager>
    {
        public static event Action OnLevelStart;
        public static event Action<LevelData> OnLevelComplete;
        
        [FormerlySerializedAs("SessionData")] 
        [SerializeField]
        private SessionData _sessionData;
        [SerializeField] private ResettableRuntimeSet _resettableRuntimeSet;
        [SerializeField] private float _levelResetDelay = 1f;
        public LevelData ActiveLevelData { get; private set; }
        private LevelFlowHandler levelFlowHandler;


        protected void OnEnable()
        {
            PlayerManager.OnPlayerDied += RestartLevel;
        }

        protected void OnDisable()
        {
            PlayerManager.OnPlayerDied -= RestartLevel;
        }
        
        protected override void Awake()
        {
            base.Awake();
            levelFlowHandler = GetComponent<LevelFlowHandler>();
        }

        protected void Start()
        {
            SetActiveLevelData();
            StartLevel();
        }

        private void StartLevel()
        {
            foreach (var resettable in _resettableRuntimeSet.GetItemList())
            {
                resettable.OnLevelReset();
            }
            OnLevelStart?.Invoke();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void RestartLevel()
        {
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(_levelResetDelay, StartLevel));
        }

        public void CompleteLevel()
        {
           if(!CheckLevelData()) return;
           ActiveLevelData.Completed = true;
           OnLevelComplete?.Invoke(ActiveLevelData);
        }

        public void LoadNextLevel()
        {
            InputManager.playerInputActions.Disable();
            levelFlowHandler.NextLevelRequest(ActiveLevelData);
        }

        public void FinishAndReturnToWorldMode()
        {
            InputManager.playerInputActions.Disable();
            levelFlowHandler.FinishLevelAndExit(ActiveLevelData);
        }

        private void SetActiveLevelData()
        {
            ActiveLevelData = GetLevelData();
            if (ActiveLevelData != null)
            {
                ActiveLevelData.Visited = true; // TO DO: set somewhere after a possible cutscene
                _sessionData.CurrentLevel = ActiveLevelData;
                _sessionData.CurrentWorld =
                    Utilities.GameSessionGetWorldDataFromLevelData(ActiveLevelData, _sessionData);
            }
        }

        private LevelData GetLevelData()
        {
            var activeScenePath = SceneManager.GetActiveScene().path;

            foreach (var worldData in _sessionData.WorldDatas)
            foreach (var levelData in worldData.LevelDatas)
                if (levelData.ScenePath == activeScenePath)
                    return levelData;
            Debug.LogWarningFormat("{0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return null;
        }

        private bool CheckLevelData()
        {
            if (ActiveLevelData != null) return true;
            Debug.LogWarningFormat("LEVEL EXIT: {0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return false;
        }
    }
}