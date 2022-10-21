using Architecture;
using Audio;
using Scriptable;
using Scriptable.Channels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button primaryButton;
        [SerializeField] private VoidEventChannelSO _returnToWorldEvent;
        [SerializeField] private VoidEventChannelSO _returnToMainMenuEvent;


        private bool paused;

        private void Start()
        {
            canvas.gameObject.SetActive(false);
            paused = false;
        }

        private void OnEnable()
        {
            LevelManager.OnLevelStart += RegisterInputActions;
        }

        private void OnDisable()
        {
            UnRegisterInputActions();
            LevelManager.OnLevelStart -= RegisterInputActions;
        }

        public void PauseGame()
        {
            if (paused) return;
            Time.timeScale = 0f;
            InputManager.ToggleActionMap(InputManager.playerInputActions.PauseMenu);
            AudioManager.Instance.PauseMusic();
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
            AudioManager.Instance.ResumeMusic();
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
#else
        UnRegisterInputActions();
        ResumeGame();
        Application.Quit();
#endif
        }

        public void ReturnToMainMenu()
        {
            ResumeGame();
            DisableInput();
            _returnToMainMenuEvent.RaiseEvent();
        }

        public void ReturnToWorldsScreen()
        {
            ResumeGame();
            DisableInput();
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

        private void DisableInput()
        {
            InputManager.playerInputActions.Disable();
        }
    }
}