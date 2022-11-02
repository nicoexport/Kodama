using UnityEngine;

namespace UI {
    public interface IUICharacter {
        void StartMoving(Transform goal);
        void StopMoving();
    }
}