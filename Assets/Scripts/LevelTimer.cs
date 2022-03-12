using UnityEngine;
using System;

public class LevelTimer : Timer
{

    public static event Action<float> OnTimerChanged;
    public static event Action<float> OnTimerFinished;

    private void OnEnable()
    {
        LevelManager.OnPlayerGainedControl += RestartTimer;
        LevelManager.OnCompleteLevel += FinishTimer;
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerGainedControl -= RestartTimer;
        LevelManager.OnCompleteLevel -= FinishTimer;
    }

    public override void FixedUpdate()
    {
        if (count)
        {
            CountUpTimer();
            OnTimerChanged?.Invoke(timer);
        }
    }

    private void FinishTimer()
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