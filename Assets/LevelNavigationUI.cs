using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class LevelNavigationUI : MonoBehaviour
{
    [SerializeField] private List<LevelNavigationSocket> sockets = new List<LevelNavigationSocket>();
    private EventSystem _eventSystem;


    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        LevelNavigationManager.OnWorldSelected += SetupSockets;
    }

    private void OnDisable()
    {
        LevelNavigationManager.OnWorldSelected -= SetupSockets;
    }

    private void SetupSockets(WorldData worldData, LevelData currentLevelData)
    {
        foreach (var socket in sockets)
        {
            socket.gameObject.SetActive(false);
        }

        var socketAmount = worldData.LevelDatas.Count;
        if (socketAmount > sockets.Count)
            return;
        
        for (var i = 0; i < socketAmount; i++)
        {
            sockets[i].gameObject.SetActive(true);
            sockets[i].SetupSocket(worldData.LevelDatas[i], i >= socketAmount - 1, i + 1);
            sockets[i].SetButtonInteractable(worldData.LevelDatas[i].Unlocked);
            if (worldData.LevelDatas[i] == currentLevelData)
            {
                SetActiveButton(sockets[i].Button);
            }
        }
        
        
    }

    private void SetActiveButton(Button button)
    {
        _eventSystem.SetSelectedGameObject(button.gameObject);
    }

}
