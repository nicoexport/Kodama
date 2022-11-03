using System;
using Kodama.Architecture;
using Kodama.Audio;
using Kodama.Data;
using Kodama.Scriptable.Channels;
using Kodama.Utility;
using Plugins.LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Kodama.UI {
    public class LevelSummary : Resettable {
        [SerializeField] private GameObject summaryButtons;
        [SerializeField] private GameObject levelFinishedTimerUI;
        [SerializeField] private float timerRevealDelay = 1f;

        [FormerlySerializedAs("tmPro")] [SerializeField]
        private TextMeshProUGUI _timerText;

        [SerializeField] private TextMeshProUGUI _recordText;
        [SerializeField] private float _recordScale;

        [Range(0.1f, 2f)] [SerializeField] private float _recordWaveLength;

        [Header("Channels")] [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;

        [SerializeField] private FloatBoolEventChannelSO _onLevelTimerFinishedChannel;

        private bool _canReturn;

        protected override void OnEnable() {
            base.OnEnable();
            _onLevelTimerFinishedChannel.OnEventRaised += DisplayLevelFinishedTimer;
            _onLevelCompleteChannel.OnEventRaised += EnableSummary;
            InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
            InputManager.playerInputActions.LevelSummary.Return.started += ReturnToWordSelect;
            _canReturn = false;
            summaryButtons.SetActive(false);
            _timerText.gameObject.SetActive(false);
            _recordText.gameObject.SetActive(false);
            levelFinishedTimerUI.SetActive(false);
        }

        protected override void OnDisable() {
            base.OnDisable();
            _onLevelTimerFinishedChannel.OnEventRaised -= DisplayLevelFinishedTimer;
            _onLevelCompleteChannel.OnEventRaised -= EnableSummary;
            InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
            InputManager.playerInputActions.LevelSummary.Return.started -= ReturnToWordSelect;
        }

        public override void OnLevelReset() {
            _canReturn = false;
            summaryButtons.SetActive(false);
            _timerText.gameObject.SetActive(false);
            _recordText.gameObject.SetActive(false);
            levelFinishedTimerUI.SetActive(false);
        }


        private void EnableSummary(LevelData levelData) {
            AudioManager.Instance.StopMusic();
            InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);

            StartCoroutine(Utilities.ActionAfterDelayEnumerator(2f, () => {
                _canReturn = true;
                ToggleButtons(InputManager.playerInputActions.LevelSummary);
            }));
        }

        private void ToggleButtons(InputActionMap actionMap) {
            InputActionMap summaryActionMap = InputManager.playerInputActions.LevelSummary;
            if (actionMap == summaryActionMap) {
                summaryButtons.SetActive(true);
            } else {
                summaryButtons.SetActive(false);
            }
        }

        private void DisplayLevelFinishedTimer(float timer, bool newRecord) =>
            StartCoroutine(Utilities.ActionAfterDelayEnumerator(timerRevealDelay, () => {
                levelFinishedTimerUI.SetActive(true);
                _timerText.gameObject.SetActive(true);
                var span = TimeSpan.FromSeconds(timer);
                _timerText.text = span.ToString("mm") + " : " + span.ToString("ss") + " : " + span.ToString("ff");
                if (newRecord) {
                    StartCoroutine(Utilities.ActionAfterDelayEnumerator(0.5f, DisplayRecordsText));
                }
            }));

        private void DisplayRecordsText() {
            _recordText.gameObject.SetActive(true);
            var rectTransform = _recordText.GetComponent<RectTransform>();
            LeanTween.scale(rectTransform, rectTransform.localScale * _recordScale, _recordWaveLength)
                .setLoopPingPong();
        }

        private void LoadNextLevel(InputAction.CallbackContext obj) {
            if (!_canReturn) {
                return;
            }

            LevelManager.Instance.LoadNextLevel();
        }

        private void ReturnToWordSelect(InputAction.CallbackContext obj) {
            if (!_canReturn) {
                return;
            }

            LevelManager.Instance.FinishAndReturnToWorldSelect();
        }
    }
}