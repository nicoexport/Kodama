using System;
using Architecture;
using Cinemachine;
using Data;
using GameManagement;
using Level.Logic;
using Player;
using UnityEngine;
using Utility;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerDied;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] TransformRuntimeSet _playerSpawnRuntimeSet;
    [SerializeField] GameObjectRuntimeSet _cinemachineRuntimeSet;
    GameObject _currentPlayer;
    PlayerLifeCycleHandler lifeCycleHandler;

    protected void OnEnable()
    {
        PlayerLifeCycleHandler.OnCharacterDeath += HandlePlayerDeath;
        LevelManager.OnLevelComplete += HandleLevelComplete;
    }
    
    protected void OnDisable()
    {
        PlayerLifeCycleHandler.OnCharacterDeath -= HandlePlayerDeath;
        LevelManager.OnLevelComplete -= HandleLevelComplete;
    }

    protected void Start()
    {
        SpawnPlayer();
        AttachCamToPlayer();
    }

    void SpawnPlayer()
    {
        _currentPlayer = Instantiate(_playerPrefab, _playerSpawnRuntimeSet.GetItemAtIndex(0).position,
            Quaternion.identity);
        InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
    }

    void AttachCamToPlayer()
    {
        if (!_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) return;
        Vector2 position = _currentPlayer.transform.position;
        cmCam.transform.position = position;
        cmCam.Follow = _currentPlayer.transform;
    }

    static void HandlePlayerDeath(Character character)
    {
        OnPlayerDied?.Invoke();
    }

    void HandleLevelComplete(LevelData levelData)
    {
        var lifeHandler = _currentPlayer.GetComponent<PlayerLifeCycleHandler>();
        var rb = _currentPlayer.GetComponent<Rigidbody2D>();
        if (lifeHandler) lifeHandler.Damageable = false;
        if (rb) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }
}
