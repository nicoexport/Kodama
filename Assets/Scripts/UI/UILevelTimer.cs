using System;
using Level.Logic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UILevelTimer : MonoBehaviour
    {
        TextMeshProUGUI textMeshPro;

        void Start()
        {
            textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = TimeSpan.FromSeconds(0f).ToString("mm\\:ss\\:ff");
        }

        void OnEnable()
        {
            LevelTimer.OnTimerChanged += UpdateTimer;
        }

        void OnDisable()
        {
            LevelTimer.OnTimerChanged -= UpdateTimer;
        }

        void UpdateTimer(float timer)
        {
            textMeshPro.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
        }
    }
}