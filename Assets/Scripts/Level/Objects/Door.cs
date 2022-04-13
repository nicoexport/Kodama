using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private GameObject door;
        [SerializeField]
        private int neededKeys = 1;
        public UnityEvent onKeysUsed;

        private void OnTriggerEnter2D(Collider2D col)
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
