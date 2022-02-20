using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelTimer : MonoBehaviour
{
    private void OnEnable()
    {
        LevelManager.onLevelTimerChanged += UpdateTimer;
    }

    private void OnDisable()
    {
        LevelManager.onLevelTimerChanged -= UpdateTimer;
    }

    private void UpdateTimer(float timer)
    {
        Debug.Log("Level Timer:_ " + timer);
    }
}
