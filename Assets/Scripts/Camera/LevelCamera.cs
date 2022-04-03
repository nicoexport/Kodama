using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelCamera : MonoBehaviour
{
    [FormerlySerializedAs("goalScale")] [SerializeField]
    private Vector3 _goalScale;
    [FormerlySerializedAs("zoomSpeed")] [SerializeField]
    private float _zoomSpeed = 1f;

    [SerializeField] private LeanTweenType _tweenType;
    
    private void OnEnable()
    {
        LevelManager.OnCompleteLevel += TweenZoom;
    }


    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= TweenZoom;
    }
    

    private void TweenZoom()
    {
        LeanTween.scale(this.gameObject, _goalScale, _zoomSpeed).setEase(_tweenType);
    }
    
}
