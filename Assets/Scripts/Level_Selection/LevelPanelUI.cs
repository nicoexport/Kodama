using System;
using Data;
using TMPro;
using UnityEngine;

namespace Level_Selection
{
    public class LevelPanelUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelNameText;
        [SerializeField] TextMeshProUGUI recordText;
        [SerializeField] string emptyRecordString;
        RectTransform _rectTransform;

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            LevelSelectSocket.OnButtonSelectedAction += UpdateLevelPanel;
        }

        void OnDisable()
        {
            LevelSelectSocket.OnButtonSelectedAction -= UpdateLevelPanel;
        }

        void UpdateLevelPanel(LevelData levelData, Transform transform1)
        {
            LeanTween.cancel(_rectTransform);
            _rectTransform.localScale = Vector3.one;
            LeanTween.scale(_rectTransform, Vector3.one * 0.9f, 1f).setEasePunch();
            levelNameText.text = levelData.LevelName;
            var record = levelData.RecordTime;
            recordText.text = float.IsPositiveInfinity(record)
                ? emptyRecordString
                : TimeSpan.FromSeconds(levelData.RecordTime).ToString("mm\\:ss\\:ff");
        }
    }
}