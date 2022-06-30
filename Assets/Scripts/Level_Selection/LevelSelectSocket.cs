using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level_Selection
{
    public class LevelSelectSocket : MonoBehaviour
    {
        [SerializeField] GameObject buttonObject;
        [SerializeField] GameObject pathObject;

        LevelData _levelData;

        public Button Button { get; private set; }

        void Awake()
        {
            Button = buttonObject.GetComponent<Button>();
        }

        public static event Action<LevelData, Transform> OnButtonSelectedAction;
        public static event Action<LevelData> OnButtonClickedAction;

        public void SetupSocket(WorldData worldData, LevelData levelData, bool lastSocket, int index)
        {
            _levelData = levelData;
            // sets the path object inactive if it is the last socket
            pathObject.SetActive(!lastSocket);
            SetUpButton(worldData, index);
        }

        void SetUpButton(WorldData worldData, int index)
        {
            var buttonTextObject = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonTextObject.text = index.ToString();

            var image = buttonObject.GetComponent<Image>();
            image.sprite = worldData.Style.MenuButtonIconSprite;
        }

        public void SetButtonInteractable(bool value)
        {
            Button.interactable = value;
            Button.enabled = value;
            var image = Button.GetComponent<Image>();
            if (value == false)
                image.color = Button.colors.disabledColor;
            else
                image.color = Button.colors.normalColor;
        }

        public void OnButtonSelected()
        {
            print("OnButtonSelected: " + _levelData.LevelName);
            OnButtonSelectedAction?.Invoke(_levelData, Button.transform);
        }

        public void OnButtonClicked()
        {
            OnButtonClickedAction?.Invoke(_levelData);
        }
    }
}