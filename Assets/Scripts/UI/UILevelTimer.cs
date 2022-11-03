using System;
using Kodama.Level.Logic;
using TMPro;
using UnityEngine;

namespace Kodama.UI {
    public class UILevelTimer : MonoBehaviour {
        private TextMeshProUGUI textMeshPro;

        private void Start() {
            textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            var span = TimeSpan.FromSeconds(0f);
            textMeshPro.text = span.ToString("mm") + " : " + span.ToString("ss") + " : " + span.ToString("ff");
        }

        private void OnEnable() => LevelTimer.OnTimerChanged += UpdateTimer;

        private void OnDisable() => LevelTimer.OnTimerChanged -= UpdateTimer;

        private void UpdateTimer(float timer) {
            // textMeshPro.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");

            var span = TimeSpan.FromSeconds(timer);
            textMeshPro.text = span.ToString("mm") + " : " + span.ToString("ss") + " : " + span.ToString("ff");
        }
    }
}