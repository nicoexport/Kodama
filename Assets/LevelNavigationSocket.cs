using UnityEngine;
using TMPro;
using System;

public class LevelNavigationSocket : MonoBehaviour
{
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject pathObject;

    public static event Action<LevelData> OnButtonSelectedAction;
    public static event Action<LevelData> OnButtonClickedAction;

    private LevelData _levelData;

    public void SetupSocket(LevelData levelData, bool lastSocket, int index)
    {
        _levelData = levelData;
        // sets the path object inactive if it is the last socket
        pathObject.SetActive(!lastSocket);

        var buttonTextObject = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonTextObject.text = index.ToString();
    }

    public void OnButtonSelected()
    {
        print("OnButtonSelected: " + _levelData.LevelName);
        OnButtonSelectedAction?.Invoke(_levelData);
    }

    public void OnButtonClicked()
    {
        OnButtonClickedAction?.Invoke(_levelData);
    }
}
