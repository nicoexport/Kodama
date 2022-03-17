using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectSocket : MonoBehaviour
{
    [SerializeField] private GameObject _buttonObject;
    [SerializeField] private GameObject _pathObject;
    public static event Action<WorldData, WorldSelectSocket> OnButtonSelectedAction;
    public static event Action<WorldData> OnButtonClickedAction;
    public Button Button { get; private set; }

    private WorldData _worldData;

    private void Awake()
    {
        Button = _buttonObject.GetComponent<Button>();
    } 

    public void SetupSocket(WorldData worldData, bool lastSocket, int index)
    {
        _worldData = worldData;
        _pathObject.SetActive(!lastSocket);
        SetUpButton(worldData, lastSocket, index);
    }

    private void SetUpButton(WorldData worldData, bool lastSocket, int index)
    {
        var image = _buttonObject.GetComponent<Image>();
        image.sprite = worldData.Style.MenuButtonIconSprite;
        
        var buttonTextObject = _buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonTextObject.text = index.ToString();
        
        Button.interactable = worldData.Unlocked;
    }

    public void OnButtonClicked()
    {
        OnButtonClickedAction?.Invoke(_worldData);
    }

    public void OnButtonSelected()
    {
        OnButtonSelectedAction?.Invoke(_worldData, this);
    }
}
