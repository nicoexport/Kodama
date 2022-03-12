using UnityEngine;
using System;
using System.Collections.Generic;


public class LevelSelectUI : MonoBehaviour
{
    [SerializeField]
    private List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();

    private void OnEnable()
    {
        LevelNavigationManager.OnWorldSelected += UpdateLevelSelectUI;
    }

    private void OnDisable()
    {
        LevelNavigationManager.OnWorldSelected -= UpdateLevelSelectUI;
    }

    private void Awake()
    {
        levelSelectButtons.Clear();
        LevelSelectButton[] buttons = GetComponentsInChildren<LevelSelectButton>();
        foreach (LevelSelectButton button in buttons)
        {
            if (!levelSelectButtons.Contains(button))
            {
                levelSelectButtons.Add(button);
                Debug.Log(button);
            }
        }
    }

    private void UpdateLevelSelectUI(WorldData worldData, LevelData levelData)
    {
        UpdateButtons(worldData);
    }

    private void UpdateButtons(WorldData worldData)
    {
        Debug.Log("Trying To Update Level Select Buttons");
        for (int i = 0; i < levelSelectButtons.Count; i++)
        {
            var button = levelSelectButtons[i];
            button.SetLevelData(worldData.LevelDatas[i]);
            button.UpdateButtonDisplay();
        }
    }
}