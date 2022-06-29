using Architecture;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cam
{
    public class LevelCamera : MonoBehaviour
    {
        [FormerlySerializedAs("goalScale")] [SerializeField]
        private Vector3 _goalScale;
        [SerializeField] float _zoomSpeed = 0.2f;
        [SerializeField] LeanTweenType _tweenType = LeanTweenType.notUsed;
    
        private void OnEnable()
        {
            LevelManager.OnLevelComplete += TweenZoom;
        }
        
        private void OnDisable()
        {
            LevelManager.OnLevelComplete -= TweenZoom;
        }
        
        private void TweenZoom(LevelData levelData)
        {
            LeanTween.scale(this.gameObject, _goalScale, _zoomSpeed).setEase(_tweenType);
        }
    
    }
}
