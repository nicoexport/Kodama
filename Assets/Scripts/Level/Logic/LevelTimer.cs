using System;
using Architecture;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Level.Logic
{
    public class LevelTimer : Timer
    {
        public static event Action<float, bool> OnTimerFinished;
        public override void FixedUpdate()
        {
            if (!count) return;
            CountUpTimer();
            OnTimerChanged?.Invoke(timer);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LevelManager.OnLevelComplete += FinishTimer;
            PlayerManager.OnPlayerDied += StopTimer;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LevelManager.OnLevelComplete -= FinishTimer;
            PlayerManager.OnPlayerDied -= StopTimer;
        }

        public static event Action<float> OnTimerChanged;

        void FinishTimer(LevelData levelData)
        {
            PauseTimer();
            var newRecord = LevelManager.Instance.ActiveLevelData.UpdateRecordTime(timer);
            OnTimerFinished?.Invoke(timer, newRecord);
        }

        void RestartTimer()
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