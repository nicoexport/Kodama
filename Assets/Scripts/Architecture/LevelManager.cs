using System;
using Audio;
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
        [FormerlySerializedAs("SessionData")] 
        [SerializeField] SessionData _sessionData;
        [SerializeField] ResettableRuntimeSet _resettableRuntimeSet;
        [SerializeField] float _levelResetDelay = 1f;
        LevelData _activeLevelData;
        LevelFlowHandler levelFlowHandler;

        public static event Action<LevelData> OnLevelComplete;
        public static event Action OnLevelStart;
        public static event Action<float, bool> OnTimerFinished;
        
        protected void OnEnable()
        {
            LevelTimer.OnTimerFinished += BroadCastFinishedTimer;
            PlayerManager.OnPlayerDied += RestartLevel;
            LevelWin.OnLevelWon += CompleteLevel;
        }

        protected void OnDisable()
        {
            LevelTimer.OnTimerFinished -= BroadCastFinishedTimer;
            PlayerManager.OnPlayerDied -= RestartLevel;
            LevelWin.OnLevelWon -= CompleteLevel;
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
        
        void StartLevel()
        {
            foreach (var resettable in _resettableRuntimeSet.GetItemList()) resettable.OnLevelReset();
            OnLevelStart?.Invoke();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void RestartLevel()
        {
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(_levelResetDelay, StartLevel));
        }

        void CompleteLevel()
        {
           if(!CheckLevelData()) return;
           _activeLevelData.Completed = true;
           OnLevelComplete?.Invoke(_activeLevelData);
           AudioManager.Instance.StopMusic();
        }

        void BroadCastFinishedTimer(float timer)
        {
            var newRecord = _activeLevelData.UpdateRecordTime(timer);
            OnTimerFinished?.Invoke(timer, newRecord);
        }

        public void LoadNextLevel()
        {
            InputManager.playerInputActions.Disable();
            levelFlowHandler.NextLevelRequest(_activeLevelData);
        }

        public void FinishAndReturnToWorldMode()
        {
            InputManager.playerInputActions.Disable();
            levelFlowHandler.FinishLevelAndExit(_activeLevelData);
        }

        void SetActiveLevelData()
        {
            _activeLevelData = GetLevelData();
            if (_activeLevelData != null)
            {
                _activeLevelData.Visited = true; // TO DO: set somewhere after a possible cutscene
                _sessionData.CurrentLevel = _activeLevelData;
                _sessionData.CurrentWorld =
                    Utilities.GameSessionGetWorldDataFromLevelData(_activeLevelData, _sessionData);
            }
        }

        LevelData GetLevelData()
        {
            var activeScenePath = SceneManager.GetActiveScene().path;

            foreach (var worldData in _sessionData.WorldDatas)
            foreach (var levelData in worldData.LevelDatas)
                if (levelData.ScenePath == activeScenePath)
                    return levelData;
            Debug.LogWarningFormat("{0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return null;
        }

        bool CheckLevelData()
        {
            if (_activeLevelData != null) return true;
            Debug.LogWarningFormat("LEVEL EXIT: {0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return false;
        }
    }
}