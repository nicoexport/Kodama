using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameModeManager : Singleton<GameModeManager>
{
    [SerializeField]
    private IGameMode _currentMode;
    private bool _isSwitching = false;

    public LevelMode levelMode { get; private set; } = new LevelMode();
    public WorldMode worldMode { get; private set; } = new WorldMode();
    public MainMenuMode mainMenuMode { get; private set; } = new MainMenuMode();

    public string _levelToLoad { get; private set; }

    private int _initialSceneIndex = 0;

    protected override void Awake()
    {
        base.Awake();
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
                _currentMode = worldMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(_initialSceneIndex, LoadSceneMode.Additive);

                break;
            // loaded from level scene
            default:
                _currentMode = levelMode;
                _currentMode.OnEditorStart();
                SceneManager.LoadScene(_initialSceneIndex, LoadSceneMode.Additive);
                break;
        }
#else
        HandleStartRequested(mainMenuMode);
#endif
        Time.timeScale = 1;
    }

    public void HandleModeStartRequested(IGameMode mode)
    {
        StartCoroutine(SwitchMode(mode));
    }

    public void HandleLevelStartRequested(LevelObject levelObject)
    {
        _levelToLoad = levelObject.ScenePath;
        HandleModeStartRequested(levelMode);
    }

    private IEnumerator SwitchMode(IGameMode mode)
    {
        Debug.Log("Trying to Switch mode to " + mode);
        yield return new WaitUntil(() => !_isSwitching);
        if (_currentMode != levelMode && _currentMode == mode) yield break;

        _isSwitching = true;
        // TO DO: Screen fadeout
        yield return ScreenFade.Instance.Require(0.5f);

        if (_currentMode != null)
            yield return _currentMode.OnEnd();
        _currentMode = mode;
        yield return _currentMode.OnStart();


        //TO DO: Screen fade in
        yield return ScreenFade.Instance.Release(0.5f);
        _isSwitching = false;
    }
}