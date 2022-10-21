using Architecture;
using Cinemachine;
using Data;
using GameManagement;
using Player;
using Scriptable.Channels;
using UnityEngine;
 
public class PlayerManager : Resettable
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
    [Header("Channels")]
    [SerializeField] private VoidEventChannelSO _onPlayerDeathChannel;
    [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;
    
    private GameObject _currentPlayer;
    private PlayerHealth _health;
    private Rigidbody2D _rb;
    private RigidbodyConstraints2D _constraints;

    protected override void OnEnable()
    {
        base.OnEnable();
        _onLevelCompleteChannel.OnEventRaised += HandleLevelComplete;
        _onPlayerDeathChannel.OnEventRaised += HandlePlayerDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _onLevelCompleteChannel.OnEventRaised -= HandleLevelComplete;
        _onPlayerDeathChannel.OnEventRaised -= HandlePlayerDeath;
    }

    public override void OnLevelReset()
    {
        RespawnPlayer();
        AttachCamToPlayer();
    }

    private void RespawnPlayer()
    {
        if (_currentPlayer == null)
        {
            _currentPlayer = Instantiate(_playerPrefab, transform.position,
            Quaternion.identity);
            CacheComponents(_currentPlayer);
            _health.Reset();
        }
        else
        {
            _currentPlayer.transform.position = transform.position;
            _health.Reset();
            _rb.constraints = _constraints;
        }

        var map = InputManager.playerInputActions.Player;
        InputManager.ToggleActionMap(map);
    }

    private void HandlePlayerDeath()
    {
        Destroy(_currentPlayer);
    }

    private void AttachCamToPlayer()
    {
        if (!_cinemachineRuntimeSet.GetItemAtIndex(0).TryGetComponent(out CinemachineVirtualCamera cmCam)) return;
        Vector2 position = _currentPlayer.transform.position;
        cmCam.transform.position = position;
        cmCam.Follow = _currentPlayer.transform;
    }
    

    private void HandleLevelComplete(LevelData levelData)
    {
        var lifeHandler = _currentPlayer.GetComponent<PlayerHealth>();
        var rb = _currentPlayer.GetComponent<Rigidbody2D>();
        if (lifeHandler) lifeHandler.Damageable = false;
        if (rb) rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void CacheComponents(GameObject player)
    {
        _health = player.GetComponent<PlayerHealth>();
        _rb = player.GetComponent<Rigidbody2D>();
        _constraints = _rb.constraints;
    }
}
