using System.Collections;
using UnityEngine;

namespace Level.Objects
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject objectPrefab;

        [SerializeField] Transform spawnPos;

        [SerializeField] float spawnInterval;

        [SerializeField] bool usesTrigger;

        [SerializeField] bool oneTime;

        bool canSpawn = true;


        bool oneTimeTriggered;
        bool spawning;

        void Start()
        {
            canSpawn = true;
            if (!usesTrigger) spawning = true;
        }

        void Update()
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
}