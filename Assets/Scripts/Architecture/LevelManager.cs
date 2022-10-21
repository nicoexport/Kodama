using System;
using Data;
using GameManagement;
using Level.Logic;
using Scriptable;
using Scriptable.Channels;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Architecture
{
    [RequireComponent(typeof(LevelFlowHandler))]
    [RequireComponent(typeof(LevelTimer))]
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private SessionData _sessionData;
        [SerializeField] private ResettableRuntimeSet _resettableRuntimeSet;
        [SerializeField] private float _levelResetDelay = 1f;

        [Header("Channels")] 
        [SerializeField] private VoidEventChannelSO _onLevelStartChannel;
        [SerializeField] private VoidEventChannelSO _onPlayerDeathChannel;
        [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;
        [SerializeField] private LevelDataEventChannelSO _onNextLevelRequestChannel;
        [SerializeField] private TransitionEventChannelSO _transitionChannel;
        
        public LevelData CurrentLevelData { get; private set; }
        private LevelFlowHandler levelFlowHandler;

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

        protected void OnEnable()
        {
            _onPlayerDeathChannel.OnEventRaised += RestartLevel;
        }

        protected void OnDisable()
        {
            _onPlayerDeathChannel.OnEventRaised -= RestartLevel;
        }

        private void StartLevel()
        {
            foreach (var resettable in _resettableRuntimeSet.GetItemList())
            {
                resettable.OnLevelReset();
            }
            _onLevelStartChannel.RaiseEvent();
        }

        private void RestartLevel()
        {
            InputManager.DisableInput();
            _transitionChannel.RaiseEvent(TransitionType.FadeOut, _levelResetDelay);
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(_levelResetDelay, ()=>
            {
                StartLevel();
                _transitionChannel.RaiseEvent(TransitionType.FadeIn, _levelResetDelay / 2f);
            }));
        }
        
        
        public void CompleteLevel()
        { 
            InputManager.DisableInput();
            if (CheckLevelData())
            {
                CurrentLevelData.Completed = true;
            }
            _onLevelCompleteChannel.RaiseEvent(CurrentLevelData);
        }

        public void LoadNextLevel()
        {
            InputManager.playerInputActions.Disable();
            if (CheckLevelData())
            {
                levelFlowHandler.NextLevelRequest(CurrentLevelData);
            }
            else
            {
                RestartLevel();
            }
        }

        public void FinishAndReturnToWorldSelect()
        {
            InputManager.playerInputActions.Disable();
            if (CheckLevelData())
            {
                levelFlowHandler.FinishLevelAndExit(CurrentLevelData);
            }
            else
            {
                RestartLevel();
            }
        }

        private void SetActiveLevelData() 
        {
            CurrentLevelData = GetCurrentLevelData();
            if (CurrentLevelData != null)
            {
                CurrentLevelData.Visited = true; // TO DO: set somewhere after a possible cutscene
                _sessionData.CurrentLevel = CurrentLevelData;
                _sessionData.CurrentWorld =
                    Utilities.GameSessionGetWorldDataFromLevelData(CurrentLevelData, _sessionData);
            }
        }

        private LevelData GetCurrentLevelData()
        {
            var activeScenePath = SceneManager.GetActiveScene().path;
            foreach (var worldData in _sessionData.WorldDatas)
            foreach (var levelData in worldData.LevelDatas)
                if (levelData.ScenePath == activeScenePath)
                    return levelData;
            Debug.LogWarningFormat("{0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return null;
        }

        public bool CheckLevelData()
        {
            if (CurrentLevelData != null) return true;
            Debug.LogWarningFormat("LEVEL EXIT: {0} NOT PART OF THE GAME DATA", SceneManager.GetActiveScene().path);
            return false;
        }
    }
}