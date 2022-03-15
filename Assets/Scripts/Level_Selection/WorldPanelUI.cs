using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _worldName;
    [SerializeField] private TextMeshProUGUI _completionText;


    private void OnEnable()
    {
        LevelNavigationManager.OnWorldSelected += UpdatePanel;
    }

    private void OnDisable()
    {
        LevelNavigationManager.OnWorldSelected -= UpdatePanel;
    }

    private void UpdatePanel(WorldData worldData, LevelData levelData)
    {
        _worldName.text = worldData.WorldName;
    }
}
