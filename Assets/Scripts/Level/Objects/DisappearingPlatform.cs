using System.Collections;
using UnityEngine;

namespace Level.Objects {
    public class DisappearingPlatform : ObjectVanisher {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Color _fadeColor;
        [SerializeField] private Color _fadeColor2;
        private Color _defaultColor;

        protected override void Awake() {
            _defaultColor = _renderer.color;
            base.Awake();
        }

        protected override IEnumerator VanishAndRespawnObject() {
            _renderer.color = _fadeColor;
            LeanTween.value(_renderer.gameObject, _fadeColor, _fadeColor2, disappearDelay);
            return base.VanishAndRespawnObject();
        }

        protected override IEnumerator ReappearEnumerator() {
            _renderer.color = _defaultColor;
            return base.ReappearEnumerator();
        }
    }
}