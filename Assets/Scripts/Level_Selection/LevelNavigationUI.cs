using System.Collections.Generic;
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


    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _camera = Camera.main;
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
            _sockets[i].gameObject.SetActive(true);
            _sockets[i].SetupSocket(worldData.LevelDatas[i], i >= socketAmount - 1, i + 1);
            _sockets[i].SetButtonInteractable(worldData.LevelDatas[i].Unlocked);
            if (worldData.LevelDatas[i] == currentLevelData)
            {
                SetActiveButton(_sockets[i].Button);
            }
        }
    }
    private void SetActiveButton(Button button)
    {
        _eventSystem.SetSelectedGameObject(button.gameObject);
    }

    private void MoveUIPlayer(LevelData levelData, LevelNavigationSocket socket)
    {
    }

}
