using UnityEngine;
using TMPro;
using System;

public class LevelNavigationSocket : MonoBehaviour
{
    [SerializeField] private GameObject _buttonObject;
    [SerializeField] private GameObject _PathObject;

    public static event Action<LevelData> OnButtonSelectedAction;

    private LevelData _levelData;
    private int _index = 99;

    public void SetupSocket(LevelData levelData, bool lastSocket, int index)
    {
        _levelData = levelData;
        _index = index;
        if (!lastSocket)
            _PathObject.SetActive(true);
        else
            _PathObject.SetActive(false);

        var buttonTextObject = _buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonTextObject.text = index.ToString();
    }

    public void OnButtonSelected()
    {
        Debug.Log("OnButtonSelected: " + _levelData.LevelName);
        OnButtonSelectedAction?.Invoke(_levelData);
    }
}
