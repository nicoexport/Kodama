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
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private TransformRuntimeSet _playerSpawnRuntimeSet;
    [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
    private GameObject _currentPlayer;
    private PlayerLifeCycleHandler lifeCycleHandler;

    protected void OnEnable()
    {
        PlayerLifeCycleHandler.OnCharacterDeathAction += HandlePlayerDeathAction;
        LevelManager.OnLevelComplete += HandleLevelComplete;
    }
    
    protected void OnDisable()
    {
        PlayerLifeCycleHandler.OnCharacterDeathAction -= HandlePlayerDeathAction;
        LevelManager.OnLevelComplete -= HandleLevelComplete;
    }

    protected void Start()
    {
        SpawnPlayer();
        AttachCamToPlayer();
    }

    private void SpawnPlayer()
    {
        _currentPlayer = Instantiate(_playerPrefab, _playerSpawnRuntimeSet.GetItemAtIndex(0).position,
            Quaternion.identity);
        InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
    }

    private void AttachCamToPlayer()
    {
        if (!_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) return;
        Vector2 position = _currentPlayer.transform.position;
        cmCam.transform.position = position;
        cmCam.Follow = _currentPlayer.transform;
    }

    private static void HandlePlayerDeathAction(Character character)
    {
        OnPlayerDied?.Invoke();
    }

    private void HandleLevelComplete(LevelData levelData)
    {
        var lifeHandler = _currentPlayer.GetComponent<PlayerLifeCycleHandler>();
        var rb = _currentPlayer.GetComponent<Rigidbody2D>();
        if (lifeHandler) lifeHandler.Damageable = false;
        if (rb) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }
}
