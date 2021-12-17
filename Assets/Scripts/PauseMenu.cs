using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    private bool paused = false;
    [SerializeField]
    RuntimeSet rtSet;
    [SerializeField]
    private Button primaryButton;
    [SerializeField]
    private int mainMenuIndex;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        RegisterInputActions();
        paused = false;
    }
    private void OnEnable()
    {
        RegisterInputActions();
    }

    private void OnDisable()
    {
        UnRegisterInputActions();
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
        if (canvas == null) return;
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
        if (canvas == null) return;
        ResumeGame();
    }

    public void QuitGame()
    {
        UnRegisterInputActions();
        ResumeGame();
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        UnRegisterInputActions();
        ResumeGame();
        SceneManager.LoadSceneAsync(mainMenuIndex);
    }

    private void RegisterInputActions()
    {
        if (rtSet.CurrentCharacter.playerInputActions == null) return;
        rtSet.CurrentCharacter.playerInputActions.Player.Pause.started += InputActionPauseGame;
        rtSet.CurrentCharacter.playerInputActions.PauseMenu.Unpause.started += InputActionResumeGame;
    }

    private void UnRegisterInputActions()
    {
        //if (rtSet.CurrentCharacter.playerInputActions == null) return;
        rtSet.CurrentCharacter.playerInputActions.Player.Pause.started -= InputActionPauseGame;
        rtSet.CurrentCharacter.playerInputActions.PauseMenu.Unpause.started -= InputActionResumeGame;
    }

}
