using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private string emptyRecordString;
    
    private void OnEnable()
    {   
        LevelNavigationSocket.OnButtonSelectedAction += UpdateLevelPanel;
    }

    private void OnDisable()
    {
        LevelNavigationSocket.OnButtonSelectedAction -= UpdateLevelPanel;
    }

    private void UpdateLevelPanel(LevelData levelData)
    {
        levelNameText.text = levelData.LevelName;
        var record = levelData.RecordTime;
        recordText.text = float.IsPositiveInfinity(record) ? emptyRecordString : TimeSpan.FromSeconds(levelData.RecordTime).ToString("mm\\:ss\\:ff");
    }
}