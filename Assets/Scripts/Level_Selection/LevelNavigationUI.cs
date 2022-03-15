using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class LevelNavigationUI : MonoBehaviour
{
    [FormerlySerializedAs("sockets")] [SerializeField] private List<LevelNavigationSocket> _sockets = new List<LevelNavigationSocket>();
    [SerializeField] private GameObject _uIPlayerObject;
    private EventSystem _eventSystem;

    private Camera _camera;
    [SerializeField] private float _playerMoveTimeInSeconds = 0.5f;


    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        LevelNavigationManager.OnWorldSelected += SetupSockets;
        LevelNavigationSocket.OnButtonSelectedAction += MoveUIPlayer;
    }

    private void OnDisable()
    {
        LevelNavigationManager.OnWorldSelected -= SetupSockets;
        LevelNavigationSocket.OnButtonSelectedAction -= MoveUIPlayer;
    }

    private void SetupSockets(WorldData worldData, LevelData currentLevelData)
    {
        foreach (var socket in _sockets)
        {
            socket.gameObject.SetActive(false);
        }

        var socketAmount = worldData.LevelDatas.Count;
        if (socketAmount > _sockets.Count)
            return;
        
        for (var i = 0; i < socketAmount; i++)
        {
            var socket = _sockets[i];
            socket.gameObject.SetActive(true);
            socket.SetupSocket(worldData.LevelDatas[i], i >= socketAmount - 1, i + 1);
            socket.SetButtonInteractable(worldData.LevelDatas[i].Unlocked);
            if (worldData.LevelDatas[i] != currentLevelData) continue;
            _uIPlayerObject.transform.position = socket.Button.transform.position;
            SetActiveButton(socket.Button);
        }
    }
    private void SetActiveButton(Button button)
    {
        _eventSystem.SetSelectedGameObject(button.gameObject);
    }

    private void MoveUIPlayer(LevelData levelData, LevelNavigationSocket socket)
    {
        LeanTween.cancel(_uIPlayerObject);
        _eventSystem.enabled = false;
        LeanTween.move(_uIPlayerObject, socket.Button.transform.position, _playerMoveTimeInSeconds)
            .setOnComplete(() => { _eventSystem.enabled = true; });
    }

}
