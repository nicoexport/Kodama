using System;
using Architecture;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility;

namespace UI
{
    public class LevelSummary : MonoBehaviour
    {
        [SerializeField] GameObject summaryButtons;

        [SerializeField] GameObject levelFinishedTimerUI;

        [SerializeField] float timerRevealDelay = 1f;

        [FormerlySerializedAs("tmPro")] [SerializeField]
        TextMeshProUGUI _timerText;

        [SerializeField] TextMeshProUGUI _recordText;
        [SerializeField] float _recordScale;

        [Range(0.1f, 2f)] [SerializeField] float _recordWaveLength;

        bool canReturn;

        void OnEnable()
        {
            LevelManager.OnTimerFinished += DisplayLevelFinishedTimer;
            LevelManager.OnLevelComplete += EnableSummary;
            InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
            InputManager.playerInputActions.LevelSummary.Return.started += ReturnToWordSelect;
            canReturn = false;
            summaryButtons.SetActive(false);
            _timerText.gameObject.SetActive(false);
            _recordText.gameObject.SetActive(false);
            levelFinishedTimerUI.SetActive(false);
        }

        void OnDisable()
        {
            LevelManager.OnTimerFinished -= DisplayLevelFinishedTimer;
            LevelManager.OnLevelComplete -= EnableSummary;
            InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
            InputManager.playerInputActions.LevelSummary.Return.started -= ReturnToWordSelect;
        }

        void EnableSummary(LevelData levelData)
        {
            InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);

            StartCoroutine(Utilities.ActionAfterDelayEnumerator(2f, () =>
            {
                canReturn = true;
                ToggleButtons(InputManager.playerInputActions.LevelSummary);
            }));
        }

        void ToggleButtons(InputActionMap actionMap)
        {
            InputActionMap summaryActionMap = InputManager.playerInputActions.LevelSummary;
            if (actionMap == summaryActionMap)
                summaryButtons.SetActive(true);
            else
                summaryButtons.SetActive(false);
        }

        void DisplayLevelFinishedTimer(float timer, bool newRecord)
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

        void DisplayRecordsText()
        {
            _recordText.gameObject.SetActive(true);
            var rectTransform = _recordText.GetComponent<RectTransform>();
            LeanTween.scale(rectTransform, rectTransform.localScale * _recordScale, _recordWaveLength)
                .setLoopPingPong();
        }

        static void LoadNextLevel(InputAction.CallbackContext obj)
        {
            InputManager.playerInputActions.Disable();
            LevelManager.Instance.LoadNextLevel();
        }

        void ReturnToWordSelect(InputAction.CallbackContext obj)
        {
            if (!canReturn)
                return;
            LevelManager.Instance.FinishAndReturnToWorldMode();
        }
    }
}