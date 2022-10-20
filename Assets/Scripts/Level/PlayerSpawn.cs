using GameManagement;
using UnityEngine;

namespace Level
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private TransformRuntimeSet playerSpawnRuntimeSet;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
        }

        private void OnEnable()
        {
            if (playerSpawnRuntimeSet.IsEmpty())
                playerSpawnRuntimeSet.AddToList(transform);
            else
                Debug.Log("multiple spawns exist");
        }

        private void OnDisable()
        {
            playerSpawnRuntimeSet.RemoveFromList(transform);
        }
    }
}