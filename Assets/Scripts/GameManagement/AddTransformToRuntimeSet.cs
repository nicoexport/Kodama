using UnityEngine;
using System.Collections.Generic;

public class AddTransformToRuntimeSet : MonoBehaviour
{
    [SerializeField]
    private TransformRuntimeSet transformRuntimeSet;

    private void OnEnable()
    {
        transformRuntimeSet.AddToList(this.transform);
    }

    private void OnDisable()
    {
        transformRuntimeSet.RemoveFromList(this.transform);
    }
}