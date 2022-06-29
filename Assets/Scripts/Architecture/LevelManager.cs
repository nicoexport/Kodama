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
        
        [Header("Level Summary")]
        [SerializeField]
        private float levelSummaryContinueDelay = 2f;
        LevelFlowHandler levelFlowHandler;
        PlayerManager _playerManager;
        LevelData _activeLevelData;
        
        public static event Action<LevelData> OnLevelComplete;
        public static event Action OnLevelStart;
        public static event Action<float, bool> OnTimerFinished;

        private void OnEnable()
        {
            LevelTimer.OnTimerFinished += BroadCastFinishedTimer;
            PlayerManager.OnPlayerDied += StartLevel;
            LevelWin.OnLevelWon += CompleteLevel;
        }

        private void OnDisable()
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

        private void Start() 
        {
            SetActiveLevelData();
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

        private void CompleteLevel()
        {
            if (_activeLevelData == null)
            {
                Debug.LogWarningFormat("LEVEL EXIT: {0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
                return;
            }
            
            _activeLevelData.Completed = true;
            OnLevelComplete?.Invoke(_activeLevelData);
            InputManager.DisableInput();
            AudioManager.Instance.StopMusic();
            EnableSummaryInput();
        }

        private void BroadCastFinishedTimer(float timer)
        {
            var newRecord = _activeLevelData.UpdateRecordTime(timer);
            OnTimerFinished?.Invoke(timer, newRecord);
        }

        private void LoadNextLevel(InputAction.CallbackContext context)
        {
            DisableSummaryInput();
            levelFlowHandler.NextLevelRequest(_activeLevelData);
        }

        private void FinishLevelAndReturnToWorldMode(InputAction.CallbackContext context)
        {
            DisableSummaryInput();
            levelFlowHandler.FinishLevelAndExit(_activeLevelData);
        }

        private void EnableSummaryInput()
        {
            InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);
            InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(levelSummaryContinueDelay, () =>
            {
                InputManager.playerInputActions.LevelSummary.Return.started += FinishLevelAndReturnToWorldMode;
            }));
        }

        private void DisableSummaryInput()
        {
            InputManager.playerInputActions.Disable();
            InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
            InputManager.playerInputActions.LevelSummary.Return.started -= FinishLevelAndReturnToWorldMode;
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

        private LevelData GetLevelData()
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
