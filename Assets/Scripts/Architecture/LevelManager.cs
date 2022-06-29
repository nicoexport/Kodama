using System;
using Audio;
using Data;
using GameManagement;
using Level.Logic;
using Scriptable;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        LevelFlowHandler levelFlowHandler;
        PlayerManager _playerManager;
        LevelData _activeLevelData;
        
        public static event Action<LevelData> OnLevelComplete;
        public static event Action OnLevelStart;
        public static event Action<float, bool> OnTimerFinished;

        protected void OnEnable()
        {
            LevelTimer.OnTimerFinished += BroadCastFinishedTimer;
            PlayerManager.OnPlayerDied += StartLevel;
            LevelWin.OnLevelWon += CompleteLevel;
        }

        protected void OnDisable()
        {
            LevelTimer.OnTimerFinished -= BroadCastFinishedTimer;
            PlayerManager.OnPlayerDied -= StartLevel;
            LevelWin.OnLevelWon -= CompleteLevel;
        }
        
        protected override void Awake()
        {
            base.Awake();
            levelFlowHandler = GetComponent<LevelFlowHandler>();
            _playerManager = GetComponent<PlayerManager>();
        }

        protected void Start() 
        {
            SetActiveLevelData();
            _playerManager.SpawnPlayer();
            StartLevel();
        }

        void StartLevel()
        {
            foreach (var resettable in  _resettableRuntimeSet.GetItemList())
            {
                resettable.ResetResettable();
            }   
            OnLevelStart?.Invoke();
        }

        void CompleteLevel()
        {
            if (_activeLevelData == null)
            {
                Debug.LogWarningFormat("LEVEL EXIT: {0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
                return;
            }
            
            _activeLevelData.Completed = true;
            InputManager.DisableInput();
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
                _sessionData.CurrentWorld = Utilities.GameSessionGetWorldDataFromLevelData(_activeLevelData, _sessionData);
            }
        }

       LevelData GetLevelData()
        {
            var activeScenePath = SceneManager.GetActiveScene().path;

            foreach (WorldData worldData in _sessionData.WorldDatas)
            {
                foreach (LevelData levelData in worldData.LevelDatas)
                {
                    if (levelData.ScenePath == activeScenePath)
                        return levelData;
                }
            }
            Debug.LogWarningFormat("{0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return null;
        }
    }
}
