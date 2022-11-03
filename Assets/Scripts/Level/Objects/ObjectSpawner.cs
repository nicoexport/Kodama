using System.Collections;
using UnityEngine;

namespace Kodama.Level.Objects {
    public class ObjectSpawner : MonoBehaviour {
        [SerializeField] private GameObject objectPrefab;

        [SerializeField] private Transform spawnPos;

        [SerializeField] private float spawnInterval;

        [SerializeField] private bool usesTrigger;

        [SerializeField] private bool oneTime;

        private bool canSpawn = true;


        private bool oneTimeTriggered;
        private bool spawning;

        private void Start() {
            canSpawn = true;
            if (!usesTrigger) {
                spawning = true;
            }
        }

        private void Update() {
            if (spawning) {
                SpawnObject();
            }
        }

        public void SpawnObject() {
            if (!canSpawn || (oneTime && oneTimeTriggered)) {
                return;
            }

            canSpawn = false;
            StartCoroutine(EnableSpawn(spawnInterval));
            Instantiate(objectPrefab, spawnPos.position, Quaternion.identity);
            if (oneTime && !oneTimeTriggered) {
                oneTimeTriggered = true;
            }
        }

        public IEnumerator EnableSpawn(float delay) {
            yield return new WaitForSeconds(delay);
            canSpawn = true;
        }

        public void SetSpawningTrue() => spawning = true;

        public void SetSpawningFalse() => spawning = false;

        public bool GetSpawning() => spawning;
    }
}