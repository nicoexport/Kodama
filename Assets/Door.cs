using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private int neededKeys = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        var inv = col.GetComponent<CharacterInventory>();
        if (inv.GetKeys() < neededKeys) return;
        inv.AddKeys(-neededKeys);
        door.SetActive(false);
    }
}
