using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Button primaryButton;
    [SerializeField]
    private VoidEventChannelSO _returnToWorldEvent;
    [SerializeField]
    private VoidEventChannelSO _returnToMainMenuEvent;


    private bool paused = false;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
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
        InputManager.ToggleActionMap(InputManager.playerInputActions.PauseMenu);
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
        InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
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
#if UNITY_EDITOR
        ResumeGame();
        return;
#else
        UnRegisterInputActions();
        ResumeGame();
        Application.Quit();
#endif
    }

    public void ReturnToMainMenu()
    {
        //UnRegisterInputActions();
        ResumeGame();
        // GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
        _returnToMainMenuEvent.RaiseEvent();
    }

    public void ReturnToWorldsScreen()
    {
        //UnRegisterInputActions();
        ResumeGame();
        _returnToWorldEvent.RaiseEvent();
    }

    private void RegisterInputActions()
    {
        InputManager.playerInputActions.Player.Pause.started += InputActionPauseGame;
        InputManager.playerInputActions.PauseMenu.Unpause.started += InputActionResumeGame;
    }

    private void UnRegisterInputActions()
    {
        InputManager.playerInputActions.Player.Pause.started -= InputActionPauseGame;
        InputManager.playerInputActions.PauseMenu.Unpause.started -= InputActionResumeGame;
    }

}
