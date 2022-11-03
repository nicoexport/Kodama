using Kodama.Player;
using UnityEngine;

namespace Kodama.Pickups {
    public class Key : PickupObject {
        [SerializeField] private int keyAmount = 1;

        public override void PickUp(Collider2D col) {
            var inv = col.GetComponent<CharacterInventory>();
            if (inv == null) {
                return;
            }

            inv.AddKeys(keyAmount);
            base.PickUp(col);
        }
    }
}