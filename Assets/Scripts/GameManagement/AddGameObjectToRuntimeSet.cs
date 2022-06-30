using UnityEngine;

namespace GameManagement
{
    public class AddGameObjectToRuntimeSet : MonoBehaviour
    {
        [SerializeField] GameObjectRuntimeSet gameObjectRuntimeSet;

        void OnEnable()
        {
            gameObjectRuntimeSet.AddToList(gameObject);
        }

        void OnDisable()
        {
            gameObjectRuntimeSet.RemoveFromList(gameObject);
        }
    }
}