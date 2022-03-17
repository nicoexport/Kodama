using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;
using Utility;

public class LevelSummaryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject summaryButtons;
    [SerializeField]
    private GameObject levelFinishedTimerUI;
    [SerializeField]
    private float timerRevealDelay = 1f;
    [SerializeField]
    private TextMeshProUGUI tmPro;

    private void OnEnable()
    {
        InputManager.OnActionMapChange += ToggleButtons;
        LevelManager.OnTimerFinished += DisplayLevelFinishedTimer;
        summaryButtons.SetActive(false);
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
        String recordString;
        if (newRecord)
            recordString = "NEW RECORD!";
        else
            recordString = "";

        StartCoroutine(Utilities.ActionAfterDelay(timerRevealDelay, () =>
        {
            levelFinishedTimerUI.SetActive(true);
            tmPro.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff") + recordString;
        }));
    }

}
