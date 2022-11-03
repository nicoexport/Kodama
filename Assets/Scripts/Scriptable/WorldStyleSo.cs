using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kodama.Scriptable {
    [Serializable]
    [CreateAssetMenu(menuName = "Style/Worldstyle")]
    public class WorldStyleSo : ScriptableObject {
        [FormerlySerializedAs("MenuButtonIcon")]
        public Sprite MenuButtonIconSprite;

        public Color MenuLightColor;
    }
}