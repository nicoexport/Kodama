using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    TransformRuntimeSet playerSpawnRuntimeSet;

    private void OnEnable()
    {
        if (playerSpawnRuntimeSet.IsEmpty())
        {
            playerSpawnRuntimeSet.AddToList(this.transform);
        }
        else
        {
            Debug.Log("multiple spawns exist");
        }
    }

    private void OnDisable()
    {
        playerSpawnRuntimeSet.RemoveFromList(this.transform);
    }
}
