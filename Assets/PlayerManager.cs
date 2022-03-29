using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private TransformRuntimeSet _playerSpawnRuntimeSet;
    [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
    [SerializeField] private float _spawnTime = 1f;
    private GameObject _currentPlayer;
    
    public IEnumerator SpawnPlayer()
    {
        _currentPlayer = Instantiate(_playerPrefab, _playerSpawnRuntimeSet.GetItemAtIndex(0).position, Quaternion.identity);
        if (_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) cmCam.Follow = _currentPlayer.transform;
        yield return new WaitForSeconds(_spawnTime);
    }
}
