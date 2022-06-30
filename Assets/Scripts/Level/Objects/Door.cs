using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class Door : MonoBehaviour
    {
        [SerializeField] GameObject door;

        [SerializeField] int neededKeys = 1;

        public UnityEvent onKeysUsed;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag != "Player") return;
            var inv = col.GetComponent<CharacterInventory>();
            if (inv.GetKeys() < neededKeys) return;
            inv.AddKeys(-neededKeys);
            onKeysUsed.Invoke();
            door.SetActive(false);
        }
    }
}