using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private SaveDataSo _sessionData;

    [Space(10)]
    public MenuState state;
    public enum MenuState { main,settings }


    [Space(10)]
    [Header("Canvases")]
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject settingsCanvas;

    [Header("Buttons")]
    [SerializeField]
    private Button primaryButtonMain;
    [SerializeField]
    private Button primaryButtonSettings;



    private void Start()
    {
        SwitchMenu("main");
    }

    public void StartGame()
    {
        _sessionData.CurrentWorld = _sessionData.WorldDatas[0];
        _sessionData.CurrentLevel = _sessionData.WorldDatas[0].LevelDatas[0];
        GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.playMode);
    }

    public void ResumeGame()
    {
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
                settingsCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                primaryButtonMain.Select();
                state = MenuState.main;
                break;
            case "settings":
                mainCanvas.SetActive(false);
                settingsCanvas.SetActive(true);
                primaryButtonSettings.Select();
                state = MenuState.settings;
                break;
            
            default:
                return;
        }
    }
    
    public void RequestResetSessionData()
    {
        GameModeManager.Instance.SetupSaveData();
    }
}
