using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utility;


public class WorldSelect : MonoBehaviour, ISelectUI
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _socketsParent;
    [SerializeField] private GameObject _socketPrefab;
    [SerializeField] private GameObject _uIPlayerObject;
    [SerializeField] private float _playerMoveTimeInSeconds = 0.5f;
    [SerializeField] private VoidEventChannelSO _returnToMainMenuChannel;

    private EventSystem  _eventSystem;
    private IUICharacter _uiPlayer;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _uiPlayer = _uIPlayerObject.GetComponent<IUICharacter>();
        _ui.SetActive(false);
    }

    private void OnEnable()
    {
        WorldSelectSocket.OnButtonSelectedAction += MoveUIPlayer;
    }

    private void OnDisable()
    {
        WorldSelectSocket.OnButtonSelectedAction -= MoveUIPlayer;
        InputManager.playerInputActions.LevelSelectUI.Exit.started -= HandleExit;
    }

    public IEnumerator OnStart(SessionData sessionData)
    {
        _ui.SetActive(true);
        yield return SetupUI(sessionData);
        _uIPlayerObject.transform.position = _eventSystem.currentSelectedGameObject.transform.position;
        MoveUIPlayer(sessionData.CurrentWorld,_eventSystem.currentSelectedGameObject.transform);
        InputManager.playerInputActions.LevelSelectUI.Exit.started += HandleExit;
    }

    public IEnumerator OnEnd()
    {
        InputManager.playerInputActions.LevelSelectUI.Exit.started -= HandleExit;
        yield return ClearSockets();
        _ui.SetActive(false);
    }
    
    private IEnumerator SetupUI(SessionData sessionData)
    {
        yield return SetupSockets(sessionData);
    }
    
    private IEnumerator SetupSockets(SessionData sessionData)
    {

        for (var index = 0; index < sessionData.WorldDatas.Count; index++)
        {
            var worldData = sessionData.WorldDatas[index];
            var socketGameObject = Instantiate(_socketPrefab, _socketsParent.transform.position, quaternion.identity,
                _socketsParent.transform);
            var socket = socketGameObject.GetComponent<WorldSelectSocket>();
            socket.SetupSocket(worldData, index >= sessionData.WorldDatas.Count  - 1, index);

            if (worldData == sessionData.CurrentWorld)
            {
                _eventSystem.SetSelectedGameObject(socket.Button.gameObject);
            }
        }
        
        yield break;
    }
    
    private void MoveUIPlayer(WorldData worldData, Transform transform1)
    {
        LeanTween.cancel(_uIPlayerObject);
        _eventSystem.enabled = false;
        _uiPlayer.StartMoving(transform1);
        LeanTween.move(_uIPlayerObject, transform1.position, _playerMoveTimeInSeconds)
            .setOnComplete(() =>
            {
                _eventSystem.enabled = true;
                _uiPlayer.StopMoving();
            });
    }
    

    private IEnumerator ClearSockets()
    {
        _socketsParent.transform.DeleteChildren();
        yield break;
    }

    public IEnumerator Reset(SessionData sessionData)
    {
        yield return ClearSockets();
        yield return OnStart(sessionData);
    }

    private void HandleExit(InputAction.CallbackContext obj)
    {
        _eventSystem.enabled = false;
        GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
    }
}
