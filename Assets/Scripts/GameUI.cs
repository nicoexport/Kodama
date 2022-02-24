using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= DisableGameUI;
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

}
