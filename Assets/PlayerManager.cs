using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action OnPlayerDied;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerDeathPrefab;
    [SerializeField] private TransformRuntimeSet _playerSpawnRuntimeSet;
    [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
    [SerializeField] private float _spawnTime = 1f;
    private GameObject _currentPlayer;
    private CharacterLifeHandler _lifeHandler;

    private void OnDisable()
    {
        if (_lifeHandler) _lifeHandler.OnCharacterDeath -= KillPlayer;
    }

    public IEnumerator SpawnPlayer()
    {
        _currentPlayer = Instantiate(_playerPrefab, _playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        _lifeHandler = _currentPlayer.GetComponent<CharacterLifeHandler>();
        _lifeHandler.OnCharacterDeath += KillPlayer;
        if (_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = _currentPlayer.transform;
        yield return new WaitForSeconds(_spawnTime);
    }

    private void KillPlayer(Character character)
    {
        Destroy(_currentPlayer);
        Instantiate(_playerDeathPrefab, character.transform.position, quaternion.identity);
        OnPlayerDied?.Invoke();
    }
}
