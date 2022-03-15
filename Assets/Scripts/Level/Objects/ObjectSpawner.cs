using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private Transform spawnPos;
    [SerializeField]
    private float spawnInterval;

    [SerializeField]
    private bool usesTrigger;
    [SerializeField]
    private bool oneTime = false;


    private bool oneTimeTriggered = false;
    private bool canSpawn = true;
    private bool spawning = false;

    private void Start()
    {
        canSpawn = true;
        if (!usesTrigger) spawning = true;
    }

    private void Update()
    {
        if (spawning) SpawnObject();
    }

    public void SpawnObject()
    {
        if (!canSpawn || (oneTime && oneTimeTriggered)) return;
        canSpawn = false;
        StartCoroutine(EnableSpawn(spawnInterval));
        Instantiate(objectPrefab, spawnPos.position, Quaternion.identity);
        if (oneTime && !oneTimeTriggered) oneTimeTriggered = true;
    }

    public IEnumerator EnableSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSpawn = true;
    }

    public void SetSpawningTrue()
    {
        spawning = true;
    }

    public void SetSpawningFalse()
    {
        spawning = false;
    }

    public bool GetSpawning()
    {
        return spawning;
    }
}
