using UnityEngine;
using System;

public class LevelTimer : Timer
{
    public delegate void LevelTimerChangedAction(float timer);
    public static event LevelTimerChangedAction OnTimerChanged;

    public override void FixedUpdate()
    {
        if (count)
        {
            CountUpTimer();
            OnTimerChanged?.Invoke(timer);
        }
    }
}