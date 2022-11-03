using UnityEngine;

namespace Kodama.GameManagement {
    public class AddTransformToRuntimeSet : MonoBehaviour {
        [SerializeField] private TransformRuntimeSet transformRuntimeSet;

        private void OnEnable() => transformRuntimeSet.AddToList(transform);

        private void OnDisable() => transformRuntimeSet.RemoveFromList(transform);
    }
}