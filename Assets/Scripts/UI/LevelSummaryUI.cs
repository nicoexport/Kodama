using System;
using Architecture;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility;

namespace UI
{
    public class LevelSummaryUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject summaryButtons;
        [SerializeField]
        private GameObject levelFinishedTimerUI;
        [SerializeField]
        private float timerRevealDelay = 1f;
        [FormerlySerializedAs("tmPro")] [SerializeField]
        private TextMeshProUGUI _timerText;

        [SerializeField] private TextMeshProUGUI _recordText;
        [SerializeField] private float _recordScale;
        [Range(0.1f,2f)]
        [SerializeField] private float _recordWaveLength;

        private void OnEnable()
        {
            InputManager.OnActionMapChange += ToggleButtons;
            LevelManager.OnTimerFinished += DisplayLevelFinishedTimer;
            summaryButtons.SetActive(false);
            _timerText.gameObject.SetActive(false);
            _recordText.gameObject.SetActive(false);
            levelFinishedTimerUI.SetActive(false);
        }

        private void OnDisable()
        {
            InputManager.OnActionMapChange -= ToggleButtons;
            LevelManager.OnTimerFinished -= DisplayLevelFinishedTimer;
        }

        private void ToggleButtons(InputActionMap actionMap)
        {
            InputActionMap summaryActionMap = InputManager.playerInputActions.LevelSummary;
            if (actionMap == summaryActionMap)
            {
                summaryButtons.SetActive(true);
            }
            else
            {
                summaryButtons.SetActive(false);
            }
        }

        private void DisplayLevelFinishedTimer(float timer, bool newRecord)
        {
            Debug.Log(timer);
            // Set Level Summary Timer Text
            // Reveal Level Timer Text (for now just enable the object)

            StartCoroutine(Utilities.ActionAfterDelayEnumerator(timerRevealDelay, () =>
            {
                levelFinishedTimerUI.SetActive(true);
                _timerText.gameObject.SetActive(true);
                _timerText.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
                if (newRecord) StartCoroutine(Utilities.ActionAfterDelayEnumerator(0.5f, DisplayRecordsText));
            }));
        }

        private void DisplayRecordsText()
        {
            _recordText.gameObject.SetActive(true);
            var rectTransform = _recordText.GetComponent<RectTransform>();
            LeanTween.scale(rectTransform, rectTransform.localScale * _recordScale, _recordWaveLength).setLoopPingPong();
        }

    }
}
