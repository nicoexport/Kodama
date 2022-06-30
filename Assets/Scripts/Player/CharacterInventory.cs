using UnityEngine;

namespace Player
{
    public class CharacterInventory : MonoBehaviour
    {
        [SerializeField] int keys;

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
}