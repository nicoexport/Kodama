using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Level;
using Level.Logic;
using Unity.Mathematics;
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
    private CharacterLifeHandler _lifeHandler;
    private bool _playerIsDead = false;


    private void OnEnable()
    {
        HellCollider.OnTriggerEntered += HandleHellColliderEntered;
        LevelBounds.OnNearingLevelBounds += HandleNearingLevelBounds;
        LevelManager.OnCompleteLevel += HandleLevelComplete;
    }


    private void OnDisable()
    {
        if (_lifeHandler) _lifeHandler.OnCharacterDeath -= KillPlayer;
        HellCollider.OnTriggerEntered -= HandleHellColliderEntered;
        LevelBounds.OnNearingLevelBounds -= HandleNearingLevelBounds;
        LevelManager.OnCompleteLevel -= HandleLevelComplete;
    }

    public IEnumerator SpawnPlayer()
    {
        _currentPlayer = Instantiate(_playerPrefab, _playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        _lifeHandler = _currentPlayer.GetComponent<CharacterLifeHandler>();
        _lifeHandler.OnCharacterDeath += KillPlayer;
        if (_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam))
        {
            Vector2 position = _currentPlayer.transform.position;
            cmCam.transform.position = position;
            cmCam.Follow = _currentPlayer.transform;
        }
        yield return new WaitForSeconds(_spawnTime);
    }

    private void KillPlayer(Character character)
    {
        var rb = _currentPlayer.GetComponent<Rigidbody2D>();
        if (rb) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        var rend = _currentPlayer.GetComponent<SpriteRenderer>();
        if (rend) rend.enabled = false;
        //Destroy(_currentPlayer);
        Instantiate(_playerDeathPrefab, character.transform.position, quaternion.identity);
        OnPlayerDied?.Invoke();
    }

    private void HandleLevelComplete()
    {
        // var pos = _currentPlayer.transform.position;
        // Destroy(_currentPlayer);
        // Instantiate(_playerWinPrefab, pos, quaternion.identity);
        var lifeHandler = _currentPlayer.GetComponent<CharacterLifeHandler>();
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
