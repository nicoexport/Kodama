using Architecture;
using Data;
using GameManagement;
using Level.Logic;
using Player;
using Scriptable.Channels;
using UnityEngine;

namespace UI
{
    public class GameUI : Resettable
    {
        [SerializeField] private CharacterRuntimeSet characterRuntimeSet;
        [SerializeField] private GameObject keyIcon;
        [SerializeField] private GameObject keyIconBackground;
        [SerializeField] private GameObject levelTimerUI;

        [Header("Channels")] 
        [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;

        protected override void OnEnable()
        {
            base.OnEnable();
            _onLevelCompleteChannel.OnEventRaised += DisableGameUI;
            HellCollider.OnTriggerEntered += FadeOut;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            _onLevelCompleteChannel.OnEventRaised -= DisableGameUI;
            HellCollider.OnTriggerEntered -= FadeOut;
        }

        public override void OnLevelReset()
        {
            EnableGameUI();
        }

        [ContextMenu("DisableGameUI")]
        private void DisableGameUI(LevelData levelData)
        {
            levelTimerUI.SetActive(false);
        }

        [ContextMenu("EnableGameUI")]
        private void EnableGameUI()
        {
            levelTimerUI.SetActive(true);
        }

        private void FadeOut(float f)
        {
            print("FADEOUT");
            var canvasGroups = GetComponentsInChildren<CanvasGroup>();
            foreach (var group in canvasGroups) LeanTween.alphaCanvas(group, 0f, 2f);
        }
    }
}