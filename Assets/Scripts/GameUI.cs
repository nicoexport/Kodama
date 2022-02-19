using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private CharacterRuntimeSet characterRuntimeSet;
    [SerializeField]
    private GameObject keyIcon;

    public void SetKeyIcon()
    {
        var inv = characterRuntimeSet.GetItemAtIndex(0).GetComponent<CharacterInventory>();
        if (inv == null) return;
        if (inv.GetKeys() > 0) keyIcon.SetActive(true);
        else keyIcon.SetActive(false);
    }

}
