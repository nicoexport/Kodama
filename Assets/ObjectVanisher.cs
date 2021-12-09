using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectVanisher : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private bool reappear;
    [SerializeField]
    private float disappearDelay;
    [SerializeField]
    private float reappearDelay;
    [SerializeField]
    private UnityEvent vanishEvent;
    [SerializeField]
    private UnityEvent vanishedEvent;
    [SerializeField]
    private UnityEvent reappearEvent;

    public void VanishObject()
    {
        StartCoroutine(VanishAndRespawnObject());
    }

    private IEnumerator VanishAndRespawnObject()
    {
        vanishEvent.Invoke();
        yield return new WaitForSeconds(disappearDelay);
        obj.SetActive(false);
        vanishedEvent.Invoke();
        if (reappear)
        {
            yield return new WaitForSeconds(reappearDelay);
            obj.SetActive(true);
            reappearEvent.Invoke();
        }
    }
}
