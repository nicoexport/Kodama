using System.Collections;
using System.Collections.Generic;
using Level;
using Level.Logic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private CharacterRuntimeSet characterRuntimeSet;
    [SerializeField]
    private GameObject keyIcon;
    [SerializeField]
    private GameObject keyIconBackground;
    [SerializeField]
    private GameObject levelTimerUI;
    
    private void OnEnable()
    {
        LevelManager.OnCompleteLevel += DisableGameUI;
        HellCollider.OnTriggerEntered += FadeOut;
        LevelBounds.OnNearingLevelBounds += HandleNearingLevelBounds;
    }


    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= DisableGameUI;
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
    private void DisableGameUI()
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
        foreach (var group in canvasGroups)
        {
            LeanTween.alphaCanvas(group, 0f, 2f);
        }
    }
    
    private void HandleNearingLevelBounds(float value)
    {
        var canvasGroups = GetComponentsInChildren<CanvasGroup>();
        foreach (var group in canvasGroups)
        {
            group.alpha = 1 - value;
        }
    }
}
