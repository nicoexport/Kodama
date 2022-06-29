using System;
using Architecture;
using Data;

namespace Level.Logic
{
    public class LevelTimer : Timer
    {

        public static event Action<float> OnTimerChanged;
        public static event Action<float> OnTimerFinished;

        private void OnEnable()
        {
            LevelManager.OnLevelStart += RestartTimer;
            LevelManager.OnLevelComplete += FinishTimer;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelStart -= RestartTimer;
            LevelManager.OnLevelComplete -= FinishTimer;
        }

        public override void FixedUpdate()
        {
            if (!count) return;
            CountUpTimer();
            OnTimerChanged?.Invoke(timer);
        }

        private void FinishTimer(LevelData levelData)
        {
            PauseTimer();
            OnTimerFinished?.Invoke(timer);
        }

        private void RestartTimer()
        {
            StopTimer();
            StartTimer();
        }

    }
}