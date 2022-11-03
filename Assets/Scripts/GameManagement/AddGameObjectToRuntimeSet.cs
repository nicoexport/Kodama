using UnityEngine;

namespace Kodama.GameManagement {
    public class AddGameObjectToRuntimeSet : MonoBehaviour {
        [SerializeField] private GameObjectRuntimeSet gameObjectRuntimeSet;

        private void OnEnable() => gameObjectRuntimeSet.AddToList(gameObject);

        private void OnDisable() => gameObjectRuntimeSet.RemoveFromList(gameObject);
    }
}