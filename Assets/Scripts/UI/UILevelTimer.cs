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
            var span = TimeSpan.FromSeconds(0f);
            textMeshPro.text = span.ToString("mm") + " : " + span.ToString("ss") + " : " + span.ToString("ff");
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
            // textMeshPro.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");

            var span = TimeSpan.FromSeconds(timer);
            textMeshPro.text = span.ToString("mm") + " : " + span.ToString("ss") + " : " + span.ToString("ff");
        }
    }
}