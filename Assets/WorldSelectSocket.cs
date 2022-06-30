using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectSocket : MonoBehaviour
{
    [SerializeField] GameObject _buttonObject;
    [SerializeField] GameObject _pathObject;
    public static event Action<WorldData, Transform> OnButtonSelectedAction;
    public static event Action<WorldData> OnButtonClickedAction;
    public Button Button { get; private set; }

    public WorldData WorldData { get; private set; }

    void Awake()
    {
        Button = _buttonObject.GetComponent<Button>();
    } 

    public void SetupSocket(WorldData worldData, bool lastSocket, int index)
    {
        WorldData = worldData;
        _pathObject.SetActive(!lastSocket);
        SetUpButton(worldData, lastSocket, index);
    }

    void SetUpButton(WorldData worldData, bool lastSocket, int index)
    {
        var image = _buttonObject.GetComponent<Image>();
        image.sprite = worldData.Style.MenuButtonIconSprite;
        
        var buttonTextObject = _buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonTextObject.text = index.ToString();
        
        Button.interactable = worldData.Unlocked;
    }

    public void OnButtonClicked()
    {
        OnButtonClickedAction?.Invoke(WorldData);
    }

    public void OnButtonSelected()
    {
        OnButtonSelectedAction?.Invoke(WorldData, Button.transform);
    }
}
