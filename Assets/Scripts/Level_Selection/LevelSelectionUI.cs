using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class LevelSelectionUI : MonoBehaviour
{
    [FormerlySerializedAs("sockets")] [SerializeField] private List<LevelNavigationSocket> _sockets = new List<LevelNavigationSocket>();
    [SerializeField] private GameObject _uIPlayerObject;
    [SerializeField] private float _playerMoveTimeInSeconds = 0.5f;
    private EventSystem _eventSystem;
    
    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        LevelNavigationSocket.OnButtonSelectedAction += MoveUIPlayer;
    }

    private void OnDisable()
    {
        LevelNavigationSocket.OnButtonSelectedAction -= MoveUIPlayer;
    }

    public IEnumerator SetupUI(WorldData worldData, LevelData currentLevelData)
    {
        SetupSockets(worldData, currentLevelData);
        yield break;
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
