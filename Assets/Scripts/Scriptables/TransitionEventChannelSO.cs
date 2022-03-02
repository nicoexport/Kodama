using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Events/Transition Event Channel")]
public class TransitionEventChannelSO : ScriptableObject
{
    public event Action<TransitionType, float> OnTransitionRequested;

    public void RaiseEvent(TransitionType transitionType, float duration)
    {
        OnTransitionRequested?.Invoke(transitionType, duration);
    }
}

public enum TransitionType
{
    FadeOut,
    FadeIn
}