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
    [RequireComponent(typeof(LevelTimer))]
    public class LevelManager : Singleton<LevelManager>
    {
        [FormerlySerializedAs("SessionData")] 
        [SerializeField] SessionData _sessionData;
        [SerializeField] ResettableRuntimeSet _resettableRuntimeSet;
        [SerializeField] VoidEventChannelSO _returnToWorldScreenEvent;



        [SerializeField] private AudioCue _levelMusicCue;
        [Header("Level Summary")]
        [SerializeField]
        private float levelSummaryContinueDelay = 2f;
        LevelFlowManager _levelFlowManager;
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
            _levelFlowManager = GetComponent<LevelFlowManager>();
            _playerManager = GetComponent<PlayerManager>();
        }

        private void Start() 
        {
            SetLevelData();
            StartLevel();

            StartLevelMusic();
        }

        void SetLevelData()
        {
            _activeLevelData = GetLevelData();
            if (_activeLevelData != null)
            {
                _activeLevelData.Visited = true; // TO DO: set somewhere after a possible cutscene
                _sessionData.CurrentLevel = _activeLevelData;
                _sessionData.CurrentWorld = Utilities.GameSessionGetWorldDataFromLevelData(_activeLevelData, _sessionData);
            }
        }

        void StartLevel()
        {
            ResetResettables();
            OnLevelStart?.Invoke();
        }

        void ResetResettables()
        {
            foreach (var resettable in  _resettableRuntimeSet.GetItemList())
            {
                resettable.ResetResettable();
            }    
        }
        
        private void StartLevelMusic()
        {
            if(!_levelMusicCue) 
                return;
            _levelMusicCue.Cue = _activeLevelData.LevelMusicAudioCueSo;
            _levelMusicCue.PlayAudioCue();
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
            _levelFlowManager.NextLevelRequest(_activeLevelData);
        }

        private void FinishLevelAndReturnToWorldMode(InputAction.CallbackContext context)
        {
            DisableSummaryInput();
            _levelFlowManager.FinishLevelAndExit(_activeLevelData);
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

        protected void OnValidate() 
        {
            if (!_levelMusicCue)
            {
                TryGetComponent(out _levelMusicCue);
            }
        }
    }
}
