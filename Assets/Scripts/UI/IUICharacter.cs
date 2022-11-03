using UnityEngine;

namespace Kodama.UI {
    public interface IUICharacter {
        void StartMoving(Transform goal);
        void StopMoving();
    }
}