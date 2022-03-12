using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class LevelNavigationUI : MonoBehaviour
{
    [SerializeField] private List<LevelNavigationSocket> sockets = new List<LevelNavigationSocket>();

    private void OnEnable()
    {
        LevelNavigationManager.OnWorldSelected += SetupSockets;
    }

    private void OnDisable()
    {
        LevelNavigationManager.OnWorldSelected -= SetupSockets;
    }

    private void SetupSockets(WorldData worldData)
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
        }
    }

}
