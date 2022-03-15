using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(LevelTimer))]
public class LevelManager : MonoBehaviour, IContextManager
{
    [field: SerializeField]
    public GameSessionDataSO gameSessionData { get; private set; }

    [Space(10)]
    [Header("Runtime Sets")]
    [SerializeField]
    private TransformRuntimeSet playerSpawnRuntimeSet;
    [SerializeField]
    private GameObjectRuntimeSet cinemachineRuntimeSet;
    [SerializeField]
    private GameObjectRuntimeSet levelWinRuntimeSet;


    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;

    [Header("Level Summary")]
    [SerializeField]
    private float levelSummaryContinueDelay = 2f;

    [SerializeField] private LevelFinishedEventChannel _levelFinishedEventChannel;
    
    public static event Action OnCompleteLevel;
    public static event Action OnPlayerGainedControl;
    public static event Action<float, bool> OnTimerFinished;

    private LevelFlowHandler _levelFlowHandler;

    private LevelData _activeLevelData;

    private void OnEnable()
    {
        Context.Instance.RegisterContextManager(this);
        LevelTimer.OnTimerFinished += BroadCastFinishedTimer;
    }

    private void OnDisable()
    {
        if (Context.Instance != null) Context.Instance.UnRegisterContextManager(this);
        LevelTimer.OnTimerFinished -= BroadCastFinishedTimer;
    }

    private void Awake()
    {
        _levelFlowHandler = GetComponent<LevelFlowHandler>();
        // Registering our Level completion for every Level Win Class in the Runtime Set
        foreach (var obj in levelWinRuntimeSet.GetItemList())
        {
            var win = obj.GetComponent<LevelWin>();
            win.OnLevelWon += CompleteLevel;
        }

    }

    private void Start() 
    {
        GetLevelData();
        _activeLevelData.Visited = true; // TO DO: set somewhere after a possible cutscene
        gameSessionData.CurrentLevel = _activeLevelData;
        gameSessionData.CurrentWorld = KodamaUtilities.GameSessionGetWorldDataFromLevelData(_activeLevelData, gameSessionData);
        var player = Instantiate(playerPrefab, playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        if (cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = player.transform;
        StartCoroutine(KodamaUtilities.ActionAfterDelay(1f, () =>
        {
            InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
            OnPlayerGainedControl?.Invoke();
        }));
    }

    public void OnGameModeStarted()
    {

    }

    private void CompleteLevel()
    {
        _activeLevelData.Completed = true;
        OnCompleteLevel?.Invoke();
        _levelFinishedEventChannel.RaiseEvent(_activeLevelData);
        InputManager.DisableInput();
        StartCoroutine(KodamaUtilities.ActionAfterDelay(levelSummaryContinueDelay, EnableSummaryInput));
        // Save Level Completion
        // Save Record
    }

    private void BroadCastFinishedTimer(float timer)
    {
        var newRecord = _activeLevelData.UpdateRecordTime(timer);
        OnTimerFinished?.Invoke(timer, newRecord);
    }

    private void LoadNextLevel(InputAction.CallbackContext context)
    {
        DisableSummaryInput();
        _levelFlowHandler.NextLevelRequest(_activeLevelData);
    }

    private void FinishLevelAndReturnToWorldMode(InputAction.CallbackContext context)
    {
        DisableSummaryInput();
        _levelFlowHandler.FinishLevelAndExit(_activeLevelData);
    }

    private void EnableSummaryInput()
    {
        InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);
        InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started += FinishLevelAndReturnToWorldMode;
    }

    private void DisableSummaryInput()
    {
        InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started -= FinishLevelAndReturnToWorldMode;
        InputManager.playerInputActions.Disable();
    }

    private void GetLevelData()
    {
        var activeScenePath = SceneManager.GetActiveScene().path;

        foreach (WorldData worldData in gameSessionData.WorldDatas)
        {
            foreach (LevelData levelData in worldData.LevelDatas)
            {
                if (levelData.ScenePath == activeScenePath)
                    _activeLevelData = levelData;
            }
        }
        Debug.Log("Got Level DAta with name: " + _activeLevelData.LevelName);
    }
}
