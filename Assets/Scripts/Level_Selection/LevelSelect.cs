using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class LevelSelect : MonoBehaviour, ISelectUI
{
    public static event Action<WorldData> OnLevelSelectStarted;
    public event Action OnReturnToWorldSelect;
    [FormerlySerializedAs("sockets")] [SerializeField] private List<LevelSelectSocket> _sockets = new List<LevelSelectSocket>();
    [SerializeField] private GameObject _uIPlayerObject;
    [SerializeField] private float _playerMoveTimeInSeconds = 0.5f;
    [SerializeField] private GameObject _ui;

    private EventSystem _eventSystem;
    private IUICharacter _uiPlayer;
    

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _uiPlayer = _uIPlayerObject.GetComponent<IUICharacter>();
        _ui.SetActive(false);
    }
    private void OnEnable()
    {
        LevelSelectSocket.OnButtonSelectedAction += MoveUIPlayer;
        InputManager.playerInputActions.LevelSelectUI.Exit.started += ReturnToWorldSelect;
    }


    private void OnDisable()
    {
        LevelSelectSocket.OnButtonSelectedAction -= MoveUIPlayer;
        InputManager.playerInputActions.LevelSelectUI.Exit.started -= ReturnToWorldSelect;
    }

    private IEnumerator SetupSockets(WorldData worldData, LevelData currentLevelData)
    {
        foreach (var socket in _sockets)
        {
            socket.gameObject.SetActive(false);
        }

        var socketAmount = worldData.LevelDatas.Count;
        if (socketAmount > _sockets.Count)
            yield break;
        
        for (var i = 0; i < socketAmount; i++)
        {
            var socket = _sockets[i];
            socket.gameObject.SetActive(true);
            socket.SetupSocket(worldData.LevelDatas[i], i >= socketAmount - 1, i + 1);
            socket.SetButtonInteractable(worldData.LevelDatas[i].Unlocked);
            if (worldData.LevelDatas[i] == currentLevelData)
            {
                _uIPlayerObject.transform.position = socket.Button.transform.position;
                SetActiveButton(socket.Button);
            }
        }
    }
    private void SetActiveButton(Button button)
    {
        _eventSystem.SetSelectedGameObject(button.gameObject);
    }

    private void ReturnToWorldSelect(InputAction.CallbackContext obj)
    {
        OnReturnToWorldSelect?.Invoke();
    }
    
    private void MoveUIPlayer(LevelData levelData, LevelSelectSocket socket)
    {
        LeanTween.cancel(_uIPlayerObject);
        _eventSystem.enabled = false;
        _uiPlayer.StartMoving(socket.Button.transform);
        LeanTween.move(_uIPlayerObject, socket.Button.transform.position, _playerMoveTimeInSeconds)
            .setOnComplete(() =>
            {
                _eventSystem.enabled = true;
                _uiPlayer.StopMoving();
            });
    }

    public IEnumerator OnStart(GameSessionDataSO sessionData)
    {
        _ui.SetActive(true);
       yield return SetupSockets(sessionData.CurrentWorld, sessionData.CurrentLevel);
       OnLevelSelectStarted?.Invoke(sessionData.CurrentWorld);
    }

    public IEnumerator OnEnd()
    {
        _ui.SetActive(false);
        yield break;
    }
}
