using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LevelSelectSocket : MonoBehaviour
{
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject pathObject;
    
    public Button Button { get; private set; }
    
    public static event Action<LevelData, LevelSelectSocket> OnButtonSelectedAction;
    public static event Action<LevelData> OnButtonClickedAction;

    private LevelData _levelData;

    private void Awake()
    {
        Button = buttonObject.GetComponent<Button>();
    }
    public void SetupSocket(WorldData worldData, LevelData levelData, bool lastSocket, int index)
    {
        _levelData = levelData;
        // sets the path object inactive if it is the last socket
        pathObject.SetActive(!lastSocket);
        SetUpButton(worldData, index);
    }

    private void SetUpButton(WorldData worldData, int index)
    {
        var buttonTextObject = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonTextObject.text = index.ToString();

        var image = buttonObject.GetComponent<Image>();
        image.sprite = worldData.Style.MenuButtonIconSprite;
    }
    public void SetButtonInteractable(bool value)
    {
        Button.interactable = value;
    }
    
    public void OnButtonSelected()
    {
        print("OnButtonSelected: " + _levelData.LevelName);
        OnButtonSelectedAction?.Invoke(_levelData, this);
    }

    public void OnButtonClicked()
    {
        OnButtonClickedAction?.Invoke(_levelData);
    }
}
