using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Objects
{
    public class ObjectVanisher : MonoBehaviour
    {
        [SerializeField] GameObject obj;

        [SerializeField] bool playOnAwake;

        [SerializeField] bool reappear;

        [SerializeField] bool cycle;

        [SerializeField] protected float disappearDelay;

        [SerializeField] float reappearDelay;

        [SerializeField] UnityEvent vanishEvent;

        [SerializeField] UnityEvent vanishedEvent;

        [SerializeField] UnityEvent reappearEvent;

        protected virtual void Awake()
        {
            if (playOnAwake) VanishObject();
            if (cycle) reappear = true;
        }

        public void VanishObject()
        {
            StartCoroutine(VanishAndRespawnObject());
        }

        protected virtual IEnumerator VanishAndRespawnObject()
        {
            vanishEvent.Invoke();
            yield return new WaitForSeconds(disappearDelay);
            obj.SetActive(false);
            vanishedEvent.Invoke();
            if (reappear) yield return ReappearEnumerator();
        }

        protected virtual IEnumerator ReappearEnumerator()
        {
            yield return new WaitForSeconds(reappearDelay);
            obj.SetActive(true);
            reappearEvent.Invoke();
            if (cycle) VanishObject();
        }
    }
}