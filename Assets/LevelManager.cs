using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelTimer))]
public class LevelManager : MonoBehaviour, IContextManager
{
    [field: SerializeField]
    public LevelObject levelObject { get; private set; }
    [Space(10)]
    [Header("Runtime Sets")]
    [SerializeField]
    private CharacterRuntimeSet playerRuntimeSet;
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

    public static event Action OnCompleteLevel;
    public static event Action OnPlayerGainedControll;
    public static event Action<float> OnTimerFinished;

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
        StartCoroutine(KodamaUtilities.ActionAfterDelay(levelSummaryContinueDelay, () => { EnableLevelSummaryInput(); }));
        // Save Level Completion
        // Save Record
    }

    private void BroadCastFinishedTimer(float timer)
    {
        OnTimerFinished?.Invoke(timer);
    }

    private void LoadNextLevel(InputAction.CallbackContext context)
    {
        // TO DO: Setup DataBase of Worlds and Levels and determine next Level to load that way
        Debug.Log("LoadNextlevel");
        InputManager.playerInputActions.Disable();
        InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started -= ReturnToLevelSelect;

        GameModeManager.Instance.HandleLevelStartRequested(levelObject);
    }

    private void ReturnToLevelSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Return to Level select");
        // Open "Are you sure Dialouge"
        InputManager.playerInputActions.Disable();
        InputManager.playerInputActions.LevelSummary.Continue.started -= LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started -= ReturnToLevelSelect;
        GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.worldMode);
    }

    private void EnableLevelSummaryInput()
    {
        InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);
        InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
        InputManager.playerInputActions.LevelSummary.Return.started += ReturnToLevelSelect;
    }

}
