using Kodama.Architecture;
using Kodama.Audio;
using Kodama.SaveLoad;
using Kodama.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Kodama.UI {
    public class MainMenu : MonoBehaviour {
        public enum MenuState {
            main,
            settings,
            deleteFile
        }

        [SerializeField] private SessionData _sessionData;

        [Space(10)] public MenuState state;


        [Space(10)] [Header("Canvases")] [SerializeField]
        private GameObject mainCanvas;

        [SerializeField] private GameObject settingsCanvas;

        [SerializeField] private GameObject _deleteSaveCanvas;

        [Header("Buttons")] [SerializeField] private Button primaryButtonMain;

        [SerializeField] private Button primaryButtonSettings;

        [SerializeField] private Button _primaryButtonDeleteSave;

        private void Awake() => InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSelectUI);

        private void Start() => SwitchMenu("main");

        public void StartGame() {
            GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.playMode);
            AudioManager.Instance.StopMusic();
        }

        public void ResumeGame() => StartGame();

        public void QuitGame() => Application.Quit();

        public void SwitchMenu(string menuName) {
            switch (menuName) {
                case "main":
                    settingsCanvas.SetActive(false);
                    mainCanvas.SetActive(true);
                    _deleteSaveCanvas.SetActive(false);
                    primaryButtonMain.Select();
                    state = MenuState.main;
                    break;
                case "settings":
                    mainCanvas.SetActive(false);
                    settingsCanvas.SetActive(true);
                    _deleteSaveCanvas.SetActive(false);
                    primaryButtonSettings.Select();
                    state = MenuState.settings;
                    break;

                case "deleteFile":
                    mainCanvas.SetActive(false);
                    settingsCanvas.SetActive(false);
                    _deleteSaveCanvas.SetActive(true);
                    _primaryButtonDeleteSave.Select();
                    state = MenuState.deleteFile;
                    break;

                default:
                    return;
            }
        }

        public void RequestResetSessionData() {
            print("TO DO: CLEARING SAVE DATA");
            SaveManager.Instance.OnDelete();
        }
    }
}