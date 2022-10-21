using System;
using Architecture;
using Data;
using Scriptable.Channels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Level.Logic
{
    public class LevelTimer : Timer
    {
        [Header("Channels")]
        [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;
        [SerializeField] private FloatBoolEventChannelSO _onLevelTimerFinished;
        
        public override void FixedUpdate()
        {
            if (!count) return;
            CountUpTimer();
            OnTimerChanged?.Invoke(timer);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _onLevelCompleteChannel.OnEventRaised += FinishTimer;
            // PlayerManager.OnPlayerDied += StopTimer;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _onLevelCompleteChannel.OnEventRaised -= FinishTimer;
            // PlayerManager.OnPlayerDied -= StopTimer;
        }

        public static event Action<float> OnTimerChanged;

        private void FinishTimer(LevelData levelData)
        {
            PauseTimer();
            bool newRecord = false;
            if (LevelManager.Instance.CheckLevelData())
            {
               newRecord = LevelManager.Instance.CurrentLevelData.UpdateRecordTime(timer);
            }
            else
            {
                newRecord = true;
            }
            _onLevelTimerFinished.RaiseEvent(timer, newRecord);
        }

        private void RestartTimer()
        {
            StopTimer();
            StartTimer();
        }
        
        public override void OnLevelReset()
        {
            RestartTimer();
        }
    }
}