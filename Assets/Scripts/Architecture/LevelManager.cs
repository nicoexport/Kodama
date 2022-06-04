using System;
using System.Collections;
using Audio;
using Data;
using GameManagement;
using Level.Logic;
using Scriptable;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utility;

namespace Architecture
{
    [RequireComponent(typeof(LevelTimer))]
    public class LevelManager : Singleton<LevelManager>, IContextManager
    {
        [field: SerializeField]
        public SessionData SessionData { get; private set; }

        [Space(10)]
        [Header("Runtime Sets")]
        [SerializeField]
        private TransformRuntimeSet playerSpawnRuntimeSet;
        [SerializeField]
        private GameObjectRuntimeSet cinemachineRuntimeSet;
        [SerializeField]
        private GameObjectRuntimeSet levelWinRuntimeSet;

        [SerializeField] private AudioCue _levelMusicCue;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject playerPrefab;

        [Header("Level Summary")]
        [SerializeField]
        private float levelSummaryContinueDelay = 2f;

        [SerializeField] private LevelFinishedEventChannel _levelFinishedEventChannel;
    
        public static event Action OnCompleteLevel;
        public static event Action OnPlayerGainedControl;
        public static event Action<float, bool> OnTimerFinished;

        private LevelFlowManager _levelFlowManager;
        private PlayerManager _playerManager;

        private LevelData _activeLevelData;

        private void OnEnable()
        {
            Context.Instance.RegisterContextManager(this);
            LevelTimer.OnTimerFinished += BroadCastFinishedTimer;
        }

        private void OnDisable()
        {
            if (Context.Instance != null) Context.Instance.UnRegisterContextManager(this);
            LevelTimer.OnTimerFinished -= BroadCastFinishedTimer;
        }

        private void Awake()
        {
            _levelFlowManager = GetComponent<LevelFlowManager>();
            _playerManager = GetComponent<PlayerManager>();
            
            foreach (var obj in levelWinRuntimeSet.GetItemList())
            {
                var win = obj.GetComponent<LevelWin>();
                win.OnLevelWon += CompleteLevel;
            }
        }

        private void Start() 
        {
            _activeLevelData = GetLevelData();
            if (_activeLevelData != null)
            {
                _activeLevelData.Visited = true; // TO DO: set somewhere after a possible cutscene
                SessionData.CurrentLevel = _activeLevelData;
                SessionData.CurrentWorld = Utilities.GameSessionGetWorldDataFromLevelData(_activeLevelData, SessionData);
            }

            StartLevelMusic();
            StartCoroutine(SpawnPlayerEnumerator());
        }

        private void StartLevelMusic()
        {
            if(!_levelMusicCue) 
                return;
            _levelMusicCue.Cue = _activeLevelData.LevelMusicAudioCueSo;
            _levelMusicCue.PlayAudioCue();
        }

        public void OnGameModeStarted()
        {

        }

        private IEnumerator SpawnPlayerEnumerator()
        {
            yield return _playerManager.SpawnPlayer();
            InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
            OnPlayerGainedControl?.Invoke();
        }

        private void CompleteLevel()
        {
            if (_activeLevelData == null)
            {
                Debug.LogWarningFormat("LEVEL EXIT: {0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
                return;
            }
            _activeLevelData.Completed = true;
            OnCompleteLevel?.Invoke();
            _levelFinishedEventChannel.RaiseEvent(_activeLevelData);
            InputManager.DisableInput();
            AudioManager.Instance.StopMusic();
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(levelSummaryContinueDelay, EnableSummaryInput));
            // Save Level Completion
            // Save Record
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
            InputManager.playerInputActions.LevelSummary.Return.started += FinishLevelAndReturnToWorldMode;
        }

        private void DisableSummaryInput()
        {
            InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
            InputManager.playerInputActions.LevelSummary.Return.started -= FinishLevelAndReturnToWorldMode;
            InputManager.playerInputActions.Disable();
        }

        private LevelData GetLevelData()
        {
            var activeScenePath = SceneManager.GetActiveScene().path;

            foreach (WorldData worldData in SessionData.WorldDatas)
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
