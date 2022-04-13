using UnityEngine;

namespace GameManagement
{
    public class AddGameObjectToRuntimeSet : MonoBehaviour
    {
        [SerializeField]
        private GameObjectRuntimeSet gameObjectRuntimeSet;

        private void OnEnable()
        {
            gameObjectRuntimeSet.AddToList(this.gameObject);
        }

        private void OnDisable()
        {
            gameObjectRuntimeSet.RemoveFromList(this.gameObject);
        }
    }
}