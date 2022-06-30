using GameManagement;
using UnityEngine;

namespace Level
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] TransformRuntimeSet playerSpawnRuntimeSet;

        SpriteRenderer _renderer;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
        }

        void OnEnable()
        {
            if (playerSpawnRuntimeSet.IsEmpty())
                playerSpawnRuntimeSet.AddToList(transform);
            else
                Debug.Log("multiple spawns exist");
        }

        void OnDisable()
        {
            playerSpawnRuntimeSet.RemoveFromList(transform);
        }
    }
}