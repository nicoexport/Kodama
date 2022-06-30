using UnityEngine;

namespace GameManagement
{
    public class AddTransformToRuntimeSet : MonoBehaviour
    {
        [SerializeField] TransformRuntimeSet transformRuntimeSet;

        void OnEnable()
        {
            transformRuntimeSet.AddToList(transform);
        }

        void OnDisable()
        {
            transformRuntimeSet.RemoveFromList(transform);
        }
    }
}