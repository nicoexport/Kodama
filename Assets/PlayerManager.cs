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
    [SerializeField] private GameObject _playerDeathPrefab;
    [SerializeField] private GameObject _playerWinPrefab;
    [SerializeField] private TransformRuntimeSet _playerSpawnRuntimeSet;
    [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
    [SerializeField] private float _spawnTime = 1f;
    private GameObject _currentPlayer;
    private PlayerLifeCycleHandler lifeCycleHandler;
    private bool _playerIsDead = false;


    private void OnEnable()
    {
        PlayerLifeCycleHandler.OnCharacterDeath += HandlePlayerDeath; 
        HellCollider.OnTriggerEntered += HandleHellColliderEntered;
        LevelBounds.OnNearingLevelBounds += HandleNearingLevelBounds;
        LevelManager.OnLevelComplete += HandleLevelComplete;
    }
    
    private void OnDisable()
    {
        PlayerLifeCycleHandler.OnCharacterDeath -= HandlePlayerDeath; 
        HellCollider.OnTriggerEntered -= HandleHellColliderEntered;
        LevelBounds.OnNearingLevelBounds -= HandleNearingLevelBounds;
        LevelManager.OnLevelComplete -= HandleLevelComplete;
    }

    public void SpawnPlayer()
    {
        _currentPlayer = Instantiate(_playerPrefab, _playerSpawnRuntimeSet.GetItemAtIndex(0).position,
            Quaternion.identity);
        InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
        AttachCamToPlayer();
    }

    void AttachCamToPlayer()
    {
        if (!_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) return;
        Vector2 position = _currentPlayer.transform.position;
        cmCam.transform.position = position;
        cmCam.Follow = _currentPlayer.transform;
    }
    
    private static void HandlePlayerDeath(Character character)
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
    
    private void HandleHellColliderEntered(float timeToKill)
    {
        StartCoroutine(Utilities.ActionAfterDelayEnumerator(timeToKill, () => { OnPlayerDied?.Invoke(); }));
    }
    
    private void HandleNearingLevelBounds(float value)
    {
        if(value < 1f) return;
        if (_playerIsDead) return;
        OnPlayerDied?.Invoke();
        _playerIsDead = true;
    }
}
