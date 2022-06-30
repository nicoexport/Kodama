using Architecture;
using Data;
using GameManagement;
using Level.Logic;
using Player;
using UnityEngine;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] CharacterRuntimeSet characterRuntimeSet;

        [SerializeField] GameObject keyIcon;

        [SerializeField] GameObject keyIconBackground;

        [SerializeField] GameObject levelTimerUI;

        void OnEnable()
        {
            LevelManager.OnLevelComplete += DisableGameUI;
            HellCollider.OnTriggerEntered += FadeOut;
            LevelBounds.OnNearingLevelBounds += HandleNearingLevelBounds;
        }


        void OnDisable()
        {
            LevelManager.OnLevelComplete -= DisableGameUI;
            HellCollider.OnTriggerEntered -= FadeOut;
            LevelBounds.OnNearingLevelBounds -= HandleNearingLevelBounds;
        }

        public void SetKeyIcon()
        {
            var inv = characterRuntimeSet.GetItemAtIndex(0).GetComponent<CharacterInventory>();
            if (inv == null) return;
            if (inv.GetKeys() > 0) keyIcon.SetActive(true);
            else keyIcon.SetActive(false);
        }

        [ContextMenu("DisableGameUI")]
        void DisableGameUI(LevelData levelData)
        {
            keyIconBackground.SetActive(false);
            keyIcon.SetActive(false);
            levelTimerUI.SetActive(false);
        }

        [ContextMenu("EnableGameUI")]
        void EnableGameUI()
        {
            keyIconBackground.SetActive(true);
            levelTimerUI.SetActive(true);
            SetKeyIcon();
        }

        void FadeOut(float f)
        {
            print("FADEOUT");
            var canvasGroups = GetComponentsInChildren<CanvasGroup>();
            foreach (var group in canvasGroups) LeanTween.alphaCanvas(group, 0f, 2f);
        }

        void HandleNearingLevelBounds(float value)
        {
            var canvasGroups = GetComponentsInChildren<CanvasGroup>();
            foreach (var group in canvasGroups) group.alpha = 1 - value;
        }
    }
}