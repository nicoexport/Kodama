using Kodama.Data;
using Kodama.Scriptable.Channels;
using Plugins.LeanTween.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kodama.Cam {
    public class LevelCamera : MonoBehaviour {
        [FormerlySerializedAs("goalScale")] [SerializeField]
        private Vector3 _goalScale;

        [SerializeField] private float _zoomSpeed = 0.2f;
        [SerializeField] private LeanTweenType _tweenType = LeanTweenType.notUsed;

        [Header("Channels")] [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;

        protected void OnEnable() => _onLevelCompleteChannel.OnEventRaised += TweenZoom;

        protected void OnDisable() => _onLevelCompleteChannel.OnEventRaised -= TweenZoom;

        private void TweenZoom(LevelData levelData) =>
            LeanTween.scale(gameObject, _goalScale, _zoomSpeed).setEase(_tweenType);
    }
}