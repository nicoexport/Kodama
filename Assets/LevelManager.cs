using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

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

    public delegate void LevelTimerChangedAction(float timer);
    public static event LevelTimerChangedAction onLevelTimerChanged;

    public static event Action OnCompleteLevel;
    public static event Action OnPlayerGainedControll;

    private bool count = false;
    private float levelTimer = 0.0f;


    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;

    private void Awake()
    {
        GameObject player = Instantiate(playerPrefab, playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        if (cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = player.transform;
        // Make a Level Start Function / Event so things can react to the point of time where the player gains controll
        StartCoroutine(KodamaUtilities.ActionAfterDelay(2f, () =>
        {
            StartLevelTimer();
            playerRuntimeSet.GetItemAtIndex(0).GivePlayerControll();
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
        PauseLevelTimer();
        // wait for Winning Animation to finish
    }

    private void StartLevelTimer()
    {
        count = true;
    }

    private void PauseLevelTimer()
    {
        count = false;
    }

    private void StopLevelTimer()
    {
        levelTimer = 0.0f;
        count = false;
    }

    private void CountUpLevelTimer()
    {
        levelTimer += Time.deltaTime;
    }

    private void Update()
    {
        if (count)
        {
            CountUpLevelTimer();
            onLevelTimerChanged?.Invoke(levelTimer);
        }
    }


}
