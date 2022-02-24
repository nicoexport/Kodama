
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
        LevelTimer.OnTimerChanged += UpdateTimer;
    }

    private void OnDisable()
    {
        LevelTimer.OnTimerChanged -= UpdateTimer;
    }

    private void UpdateTimer(float timer)
    {
        textMeshPro.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
    }
}
