using UnityEngine;
using System;

public abstract class Timer : MonoBehaviour
{
    protected bool count;
    protected float timer = 0.0f;

    public virtual void StartTimer()
    {
        count = true;
    }

    public virtual void PauseTimer()
    {
        count = false;
    }

    public virtual void StopTimer()
    {
        timer = 0.0f;
        count = false;
    }

    public virtual void CountUpTimer()
    {
        timer += Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {
        if (count)
        {
            CountUpTimer();
        }
    }
}