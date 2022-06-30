using Architecture;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cam
{
    public class LevelCamera : MonoBehaviour
    {
        [FormerlySerializedAs("goalScale")] [SerializeField]
        Vector3 _goalScale;

        [SerializeField] float _zoomSpeed = 0.2f;
        [SerializeField] LeanTweenType _tweenType = LeanTweenType.notUsed;

        protected void OnEnable()
        {
            LevelManager.OnLevelComplete += TweenZoom;
        }

        protected void OnDisable()
        {
            LevelManager.OnLevelComplete -= TweenZoom;
        }

        void TweenZoom(LevelData levelData)
        {
            LeanTween.scale(gameObject, _goalScale, _zoomSpeed).setEase(_tweenType);
        }
    }
}