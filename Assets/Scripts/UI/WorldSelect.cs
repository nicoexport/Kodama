using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;


public class WorldSelect : MonoBehaviour, ISelectUI
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _socketsParent;
    [SerializeField] private GameObject _socketPrefab;

    private EventSystem  _eventSystem;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _ui.SetActive(false);
    }

    public IEnumerator OnStart(GameSessionDataSO sessionData)
    {
        _ui.SetActive(true);
        yield return SetupUI(sessionData);
    }

    public IEnumerator OnEnd()
    {
        yield return ClearSockets();
        _ui.SetActive(false);
    }
    
    private IEnumerator SetupUI(GameSessionDataSO gameSessionData)
    {
        yield return SetupSockets(gameSessionData);
    }
    
    private IEnumerator SetupSockets(GameSessionDataSO gameSessionData)
    {
        for (var index = 0; index < gameSessionData.WorldDatas.Count; index++)
        {
            var worldData = gameSessionData.WorldDatas[index];
            var socketGameObject = Instantiate(_socketPrefab, _socketsParent.transform.position, quaternion.identity,
                _socketsParent.transform);
            var socket = socketGameObject.GetComponent<WorldSelectSocket>();
            socket.SetupSocket(worldData, index >= gameSessionData.WorldDatas.Count  - 1, index);
            
            if (worldData == gameSessionData.CurrentWorld)
            {
                _eventSystem.SetSelectedGameObject(socket.Button.gameObject);
            }
        }
        yield break;
    }

    private IEnumerator ClearSockets()
    {
        _socketsParent.transform.DeleteChildren();
        yield break;
    }

    public IEnumerator Reset(GameSessionDataSO sessionDataSo)
    {
        yield return ClearSockets();
        yield return OnStart(sessionDataSo);
    }
}
