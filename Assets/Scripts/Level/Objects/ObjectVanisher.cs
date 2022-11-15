using System.Collections;
using Kodama.Architecture;
using UnityEngine;
using UnityEngine.Events;

namespace Kodama.Level.Objects {
    public class ObjectVanisher : Resettable {
        [SerializeField] private GameObject obj;
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool reappear;
        [SerializeField] private bool cycle;
        [SerializeField] protected float disappearDelay;
        [SerializeField] private float reappearDelay;

        [SerializeField] private UnityEvent onLevelStart;
        [SerializeField] private UnityEvent vanishEvent;
        [SerializeField] private UnityEvent vanishedEvent;
        [SerializeField] private UnityEvent reappearingEvent;
        [SerializeField] private UnityEvent reappearEvent;

        private Coroutine vanishCoroutine;

        protected virtual void Awake() {
            if (playOnAwake) {
                VanishObject();
            }

            if (cycle) {
                reappear = true;
            }
        }

        public void VanishObject() => vanishCoroutine = StartCoroutine(VanishAndRespawnObject());

        protected virtual IEnumerator VanishAndRespawnObject() {
            vanishEvent.Invoke();
            yield return new WaitForSeconds(disappearDelay);
            obj.SetActive(false);
            vanishedEvent.Invoke();
            if (reappear) {
                reappearingEvent?.Invoke();
                yield return new WaitForSeconds(reappearDelay);
                obj.SetActive(true);
                reappearEvent.Invoke();
                if (cycle) {
                    VanishObject();
                }
            }
        }

        public override void OnLevelReset() {
            onLevelStart?.Invoke();
            obj.SetActive(true);
            if (vanishCoroutine != null) {
                StopCoroutine(vanishCoroutine);
            }
        }
    }
}