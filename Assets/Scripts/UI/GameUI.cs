using Architecture;
using Data;
using GameManagement;
using Level.Logic;
using Player;
using Scriptable.Channels;
using UnityEngine;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private CharacterRuntimeSet characterRuntimeSet;
        [SerializeField] private GameObject keyIcon;
        [SerializeField] private GameObject keyIconBackground;
        [SerializeField] private GameObject levelTimerUI;

        [Header("Channels")] 
        [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;

        private void OnEnable()
        {
            _onLevelCompleteChannel.OnEventRaised += DisableGameUI;
            HellCollider.OnTriggerEntered += FadeOut;
        }


        private void OnDisable()
        {
            _onLevelCompleteChannel.OnEventRaised -= DisableGameUI;
            HellCollider.OnTriggerEntered -= FadeOut;
        }

        public void SetKeyIcon()
        {
            var inv = characterRuntimeSet.GetItemAtIndex(0).GetComponent<CharacterInventory>();
            if (inv == null) return;
            if (inv.GetKeys() > 0) keyIcon.SetActive(true);
            else keyIcon.SetActive(false);
        }

        [ContextMenu("DisableGameUI")]
        private void DisableGameUI(LevelData levelData)
        {
            keyIconBackground.SetActive(false);
            keyIcon.SetActive(false);
            levelTimerUI.SetActive(false);
        }

        [ContextMenu("EnableGameUI")]
        private void EnableGameUI()
        {
            keyIconBackground.SetActive(true);
            levelTimerUI.SetActive(true);
            SetKeyIcon();
        }

        private void FadeOut(float f)
        {
            print("FADEOUT");
            var canvasGroups = GetComponentsInChildren<CanvasGroup>();
            foreach (var group in canvasGroups) LeanTween.alphaCanvas(group, 0f, 2f);
        }
    }
}