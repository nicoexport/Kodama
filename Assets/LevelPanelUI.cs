using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelPanelUI : MonoBehaviour
{
    private TextMeshProUGUI _panelText;

    private void Awake()
    {
        _panelText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LevelSelectButton.OnButtonSelectedAction += UpdateLevelPanel;
    }

    private void OnDisable()
    {
        LevelSelectButton.OnButtonSelectedAction -= UpdateLevelPanel;
    }

    private void UpdateLevelPanel(LevelData levelData)
    {
        _panelText.text = levelData.LevelName;
    }
}
