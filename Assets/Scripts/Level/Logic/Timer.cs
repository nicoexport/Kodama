using Kodama.Architecture;
using UnityEngine;

namespace Kodama.Level.Logic {
    public abstract class Timer : Resettable {
        protected bool count;
        protected float timer;

        public virtual void FixedUpdate() {
            if (count) {
                CountUpTimer();
            }
        }

        public virtual void StartTimer() => count = true;

        public virtual void PauseTimer() => count = false;

        public virtual void StopTimer() {
            timer = 0.0f;
            count = false;
        }

        public virtual void CountUpTimer() => timer += Time.deltaTime;
    }
}