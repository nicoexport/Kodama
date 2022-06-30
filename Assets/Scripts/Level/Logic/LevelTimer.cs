using System;
using Architecture;
using Data;

namespace Level.Logic
{
    public class LevelTimer : Timer
    {
        public override void FixedUpdate()
        {
            if (!count) return;
            CountUpTimer();
            OnTimerChanged?.Invoke(timer);
        }

        void OnEnable()
        {
            LevelManager.OnLevelStart += RestartTimer;
            LevelManager.OnLevelComplete += FinishTimer;
        }

        void OnDisable()
        {
            LevelManager.OnLevelStart -= RestartTimer;
            LevelManager.OnLevelComplete -= FinishTimer;
        }

        public static event Action<float> OnTimerChanged;
        public static event Action<float> OnTimerFinished;

        void FinishTimer(LevelData levelData)
        {
            PauseTimer();
            OnTimerFinished?.Invoke(timer);
        }

        void RestartTimer()
        {
            StopTimer();
            StartTimer();
        }
    }
}