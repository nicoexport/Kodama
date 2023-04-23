using UnityEngine;

namespace Kodama.Player {
    public class ShieldVisuals : MonoBehaviour {
        [SerializeField] private GameObject visualObject;
        
        public void Enable() {
            visualObject.SetActive(true);
        }

        public void Disable() {
            visualObject.SetActive(false);
        }
    }
}