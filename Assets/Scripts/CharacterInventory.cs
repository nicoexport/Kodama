using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField]
    private int keys = 0;

    public void AddKeys(int amount)
    {
        keys += amount;
    }

    public void SetKeys(int amount)
    {
        keys = amount;
    }

    public int GetKeys()
    {
        return keys;
    }
}
