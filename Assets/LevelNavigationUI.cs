using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelNavigationUI : MonoBehaviour
{
    [SerializeField] private WorldDataSO _worldDataSO;
    [SerializeField] private GameSessionDataSO _sessionDataSO;
    [SerializeField] private List<LevelNavigationSocket> _sockets = new List<LevelNavigationSocket>();

    private void Awake()
    {
        SetupSockets(KodamaUtilities.GetWorldDataFromWorldDataSO(_worldDataSO, _sessionDataSO));
    }

    private void OnEnable()
    {
        LevelSelectManager.OnWorldSelected += SetupSockets;
    }

    private void OnDisable()
    {
        LevelSelectManager.OnWorldSelected -= SetupSockets;
    }

    private void SetupSockets(WorldData worldData)
    {
        foreach (LevelNavigationSocket socket in _sockets)
        {
            socket.gameObject.SetActive(false);
        }

        int socketAmount = worldData.LevelDatas.Count;
        if (socketAmount > _sockets.Count)
            return;
        for (int i = 0; i < socketAmount; i++)
        {
            _sockets[i].gameObject.SetActive(true);
            if (i >= socketAmount - 1)
                _sockets[i].SetupSocket(worldData.LevelDatas[i], true, i + 1);
            else
                _sockets[i].SetupSocket(worldData.LevelDatas[i], false, i + 1);
        }
    }

}
