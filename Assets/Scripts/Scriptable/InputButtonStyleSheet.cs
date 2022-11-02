using System;
using UnityEngine;

namespace Scriptable {
    [CreateAssetMenu(fileName = "new Input Button Stylesheet", menuName = "Style/InputButtonStyle", order = 0)]
    public class InputButtonStyleSheet : ScriptableObject {
        public InputButtonStyle Keyboard;
        public InputButtonStyle Playstation;
        public InputButtonStyle Xbox;
        public InputButtonStyle Nintendo;
    }

    [Serializable]
    public class InputButtonStyle {
        public Sprite Up;
        public Sprite Right;
        public Sprite Left;
        public Sprite Down;
        public Sprite North;
        public Sprite East;
        public Sprite West;
        public Sprite South;
    }
}