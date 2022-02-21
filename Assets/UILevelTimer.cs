
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UILevelTimer : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = TimeSpan.FromSeconds(0f).ToString("mm\\:ss\\:ff");
    }

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
        textMeshPro.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
    }
}
