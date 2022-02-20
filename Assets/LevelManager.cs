using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    public delegate void LevelTimerChangedAction(float timer);
    public static event LevelTimerChangedAction onLevelTimerChanged;

    private bool count = false;
    private float levelTimer = 0.0f;


    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;

    private void Awake()
    {
        GameObject player = Instantiate(playerPrefab, playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        if (cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = player.transform;
        StartCoroutine(KodamaUtilities.ActionAfterDelay(2f, () => { StartLevelTimer(); }));
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
