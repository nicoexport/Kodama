using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    private bool paused = false;
    [SerializeField]
    RuntimeSet rtSet;
    [SerializeField]
    private Button primaryButton;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        paused = false;
    }
    private void OnEnable()
    {
        rtSet.CurrentCharacter.playerInputActions.Player.Pause.started += InputActionPauseGame;
        rtSet.CurrentCharacter.playerInputActions.PauseMenu.Unpause.started += InputActionResumeGame;
    }

    private void OnDisable()
    {
        rtSet.CurrentCharacter.playerInputActions.Player.Pause.started -= InputActionPauseGame;
        rtSet.CurrentCharacter.playerInputActions.PauseMenu.Unpause.started -= InputActionResumeGame;
    }

    public void PauseGame()
    {
        if (paused) return;
        Time.timeScale = 0f;
        rtSet.CurrentCharacter.playerInputActions.PauseMenu.Enable();
        rtSet.CurrentCharacter.playerInputActions.Player.Disable();
        canvas.gameObject.SetActive(true);
        primaryButton.Select();
        paused = true;
    }

    private void InputActionPauseGame(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    public void ResumeGame()
    {
        if (!paused) return;
        Time.timeScale = 1f;
        rtSet.CurrentCharacter.playerInputActions.PauseMenu.Disable();
        rtSet.CurrentCharacter.playerInputActions.Player.Enable();
        canvas.gameObject.SetActive(false);
        paused = false;
    }

    private void InputActionResumeGame(InputAction.CallbackContext context)
    {
        ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
