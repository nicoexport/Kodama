using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameModeManager : Singleton<GameModeManager>
{

    [SerializeField]
    private TransitionEventChannelSO _transitionEventChannel;
    [SerializeField]
    private GameSessionDataSO _sessionData;


    [HideInInspector] public string MainMenuScenePath; //{ get; private set; }
    [HideInInspector] public string WorldsScenePath; //{ get; private set; }

    public MainMenuMode mainMenuMode { get; private set; }
    public PlayMode playMode { get; private set; }
    public string _levelToLoad { get; private set; }


    private IGameMode _currentMode;
    private bool _isSwitching = false;
    private int _initialSceneIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        playMode = new PlayMode(WorldsScenePath);
        mainMenuMode = new MainMenuMode(MainMenuScenePath);
        _sessionData.WorldsScenePath = WorldsScenePath;

        Time.timeScale = 0;

#if UNITY_EDITOR
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            // TO DO: As soon as we have a InitialScene that needs to be loaded Additive to our Level Scenes we need to load it when we start the game not from the initial scene
            // loaded from InitialScene
            case 0:
                HandleModeStartRequested(mainMenuMode);
                break;

            // loaded from MainMenu scene
            case 1:
                _currentMode = mainMenuMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(_initialSceneIndex, LoadSceneMode.Additive);
                break;

            // loaded from world scene
            case 2:
                _currentMode = playMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(_initialSceneIndex, LoadSceneMode.Additive);

                break;
            // loaded from level scene
            default:
                _currentMode = playMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(_initialSceneIndex, LoadSceneMode.Additive);
                break;
        }
#else
        HandleModeStartRequested(mainMenuMode);
#endif
        Time.timeScale = 1;
    }

    public void HandleModeStartRequested(IGameMode mode)
    {
        StartCoroutine(SwitchMode(mode));
    }

    public void HandleLevelStartRequested(LevelObject levelObject)
    {
    }

    private IEnumerator SwitchMode(IGameMode mode)
    {
        Debug.Log("Trying to Switch mode to " + mode);
        yield return new WaitUntil(() => !_isSwitching);
        if (_currentMode == mode) yield break;

        _isSwitching = true;

        _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 1f);
        yield return new WaitForSeconds(1f);

        if (_currentMode != null)
            yield return _currentMode.OnEnd();

        _currentMode = mode;
        yield return _currentMode.OnStart();

        _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, 1f);
        yield return new WaitForSeconds(1f);

        _isSwitching = false;
    }



    // TO DO: NEEDS TO BE IMPLEMENTED CORRECTLY INTO A SAVE/LOADD SYSTEM
    public void ResetSessionData()
    {
        var sessionData = _sessionData;
        foreach (WorldDataSO world in sessionData.WorldDatas)
        {
            world.Visited = false;
            world.Completed = false;

            foreach (LevelDataSO level in world.LevelDatas)
            {
                level.Visited = false;
                level.Completed = false;
                level.RecordTime = Mathf.Infinity;
            }
        }
        var currentWorld = sessionData.CurrentWorld = sessionData.WorldDatas[0];
        sessionData.CurrentLevel = currentWorld.LevelDatas[0];
    }
}