using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelTimer))]
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelObject levelObject;
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

    public static event Action OnCompleteLevel;
    public static event Action OnPlayerGainedControll;

    private LevelTimer levelTimer;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;

    private void Awake()
    {
        levelTimer = GetComponent<LevelTimer>();
        GameObject player = Instantiate(playerPrefab, playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        if (cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = player.transform;
        StartCoroutine(KodamaUtilities.ActionAfterDelay(1f, () =>
        {
            levelTimer.StartTimer();
            InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
            OnPlayerGainedControll?.Invoke();
        }));


        // Registering for every Level Win Class in the Runtime Set
        foreach (GameObject obj in levelWinRuntimeSet.GetItemList())
        {
            LevelWin win = obj.GetComponent<LevelWin>();
            win.OnLevelWon += CompleteLevel;
        }
    }

    private void CompleteLevel()
    {
        OnCompleteLevel?.Invoke();
        levelTimer.PauseTimer();
        // wait for Winning Animation to finish

        // Save Level Completion
        // Save Record


        // TO DO: Add delay for enabling the input
        // Enable Input for Summary Section TO DO: Seperate this Handling into an Input Manager Script
        var player = playerRuntimeSet.GetItemAtIndex(0);
        InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSummary);
        InputManager.playerInputActions.LevelSummary.Continue.started += LoadNextLevel;
    }

    private void LoadNextLevel(InputAction.CallbackContext context)
    {
        // TO DO: Setup DataBase of Worlds and Levels and determine next Level to load that way
        InputManager.playerInputActions.Disable();
        Debug.Log("LoadNextlevel");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

}
