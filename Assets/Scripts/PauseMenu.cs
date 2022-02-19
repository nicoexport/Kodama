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
    private CharacterRuntimeSet characterRuntimeSet;
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
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.PauseMenu.Enable();
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.Player.Disable();
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
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.PauseMenu.Disable();
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.Player.Enable();
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
        if (characterRuntimeSet.IsEmpty()) return;
        if (characterRuntimeSet.GetItemAtIndex(0).playerInputActions == null) return;
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.Player.Pause.started += InputActionPauseGame;
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.PauseMenu.Unpause.started += InputActionResumeGame;
    }

    private void UnRegisterInputActions()
    {
        if (characterRuntimeSet.IsEmpty()) return;
        //if (rtSet.CurrentCharacter.playerInputActions == null) return;
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.Player.Pause.started -= InputActionPauseGame;
        characterRuntimeSet.GetItemAtIndex(0).playerInputActions.PauseMenu.Unpause.started -= InputActionResumeGame;
    }

}
