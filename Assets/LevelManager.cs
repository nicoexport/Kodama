using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;


[RequireComponent(typeof(LevelTimer))]
public class LevelManager : MonoBehaviour, IContextManager
{

    // TO DO: eventually get the Refrence to the LevelDataSO by looking through all of them and comparing the scene Path
    [field: SerializeField]
    public LevelDataSO levelData { get; private set; }

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

    [SerializeField]
    private LoadLevelEventChannelSO _loadEventChannel;

    public static event Action OnCompleteLevel;
    public static event Action OnPlayerGainedControll;
    public static event Action<float> OnTimerFinished;

    private LevelFlowHandler _levelExitHandler;

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
        _levelExitHandler = GetComponent<LevelFlowHandler>();
        // Registering our Level completion for every Level Win Class in the Runtime Set
        foreach (GameObject obj in levelWinRuntimeSet.GetItemList())
        {
            LevelWin win = obj.GetComponent<LevelWin>();
            win.OnLevelWon += CompleteLevel;
        }
    }

    private void Start()
    {
        GameObject player = Instantiate(playerPrefab, playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        if (cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = player.transform;
        StartCoroutine(KodamaUtilities.ActionAfterDelay(1f, () =>
        {
            InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
            OnPlayerGainedControll?.Invoke();
        }));
    }

    public void OnGameModeStarted()
    {

    }

    private void CompleteLevel()
    {
        OnCompleteLevel?.Invoke();
        InputManager.DisableInput();
        // wait for Winning Animation to finish and enable LevelSummaryInput
        StartCoroutine(KodamaUtilities.ActionAfterDelay(levelSummaryContinueDelay, () => { EnableSummaryInput(); }));
        // Save Level Completion
        // Save Record
    }

    private void BroadCastFinishedTimer(float timer)
    {
        OnTimerFinished?.Invoke(timer);
    }

    private void LoadNextLevel(InputAction.CallbackContext context)
    {
        DisableSummaryInput();
        _levelExitHandler.NextLevelRequest(levelData);
    }

    private void ReturnToWorldMode(InputAction.CallbackContext context)
    {
        DisableSummaryInput();
        // return to world selection Scene
    }

    private void EnableSummaryInput()
    {
        InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);
        InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started += ReturnToWorldMode;
    }

    private void DisableSummaryInput()
    {
        InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started -= ReturnToWorldMode;
        InputManager.playerInputActions.Disable();
    }

}
