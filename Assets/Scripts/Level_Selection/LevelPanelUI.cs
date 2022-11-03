using System;
using Kodama.Data;
using Plugins.LeanTween.Framework;
using TMPro;
using UnityEngine;

namespace Kodama.Level_Selection {
    public class LevelPanelUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI recordText;
        [SerializeField] private string emptyRecordString;
        private RectTransform _rectTransform;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void OnEnable() => LevelSelectSocket.OnButtonSelectedAction += UpdateLevelPanel;

        private void OnDisable() => LevelSelectSocket.OnButtonSelectedAction -= UpdateLevelPanel;

        private void UpdateLevelPanel(LevelData levelData, Transform transform1) {
            LeanTween.cancel(_rectTransform);
            _rectTransform.localScale = Vector3.one;
            LeanTween.scale(_rectTransform, Vector3.one * 0.9f, 1f).setEasePunch();
            levelNameText.text = levelData.LevelName;
            float record = levelData.RecordTime;
            recordText.text = float.IsPositiveInfinity(record)
                ? emptyRecordString
                : TimeSpan.FromSeconds(levelData.RecordTime).ToString("mm\\:ss\\:ff");
        }
    }
}