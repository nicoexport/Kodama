using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class LevelSummaryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject summaryButtons;

    private void OnEnable()
    {
        InputManager.OnActionMapChange += ToggleButtons;
        summaryButtons.SetActive(false);
    }

    private void OnDisable()
    {
        InputManager.OnActionMapChange -= ToggleButtons;
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

}
