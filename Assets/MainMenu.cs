using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    [Space(10)]
    public MenuState state;
    public enum MenuState { main, level, settings }


    [Space(10)]
    [Header("Canvases")]
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject levelCanvas;
    [SerializeField]
    private GameObject settingsCanvas;

    [Header("Buttons")]
    [SerializeField]
    private Button primaryButtonMain;
    [SerializeField]
    private Button primaryButtonLevel;
    [SerializeField]
    private Button primaryButtonSettings;



    private void Start()
    {
        SwitchMenu("main");
    }

    public void StartGame()
    {
        // SceneManager.LoadScene(defaultLevel.ScenePath);
        GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.playMode);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchMenu(string menuname)
    {
        switch (menuname)
        {
            case "main":
                levelCanvas.SetActive(false);
                settingsCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                primaryButtonMain.Select();
                state = MenuState.main;
                break;
            case "settings":
                mainCanvas.SetActive(false);
                levelCanvas.SetActive(false);
                settingsCanvas.SetActive(true);
                primaryButtonSettings.Select();
                state = MenuState.settings;
                break;

            case "level":
                mainCanvas.SetActive(false);
                settingsCanvas.SetActive(false);
                levelCanvas.SetActive(true);
                primaryButtonLevel.Select();
                state = MenuState.level;
                break;

            default:
                return;
        }
    }

    // temporaray
    public void RequestResetSessionData()
    {
        GameModeManager.Instance.SetupSessionData();
    }
}
