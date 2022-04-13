using System;
using Data;
using TMPro;
using UnityEngine;

namespace Level_Selection
{
    public class LevelSelectButton : MonoBehaviour
    {
        private LevelData _levelData;
        private TextMeshProUGUI _buttonText;

        public static event Action<LevelData> OnButtonSelectedAction;

        private void Awake()
        {
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;
        }

        public void UpdateButtonDisplay()
        {
            Debug.Log("Trying to set button to: " + _levelData.LevelName);
            _buttonText.text = _levelData.LevelName;
        }

        public void OnButtonSelected()
        {
            Debug.Log("OnButtonSelected: " + _levelData.LevelName);
            OnButtonSelectedAction?.Invoke(_levelData);
        }
    }
}
