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
    [FormerlySerializedAs("zoomDelta")] [SerializeField]
    private float _zoomDelta = 0.001f;
    private bool _zoomIn;
    private bool _zoomOut;
    private Vector3 _gameplayScale;

    private void Awake()
    {
        _gameplayScale = transform.localScale;
        transform.localScale = _goalScale;
    }

    private void OnEnable()
    {
        LevelManager.OnCompleteLevel += StartZoomIn;
    }


    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= StartZoomIn;
    }

    private void StartZoomIn()
    {
        _zoomIn = true;
    }
    

    private void Start()
    {
        _zoomOut = true;
    }
    
    private void Zoom()
    {
        if (!_zoomIn) return;
        transform.localScale = Vector3.Lerp(transform.localScale, _goalScale, _zoomSpeed * Time.deltaTime);
        if (transform.localScale.x <= (_goalScale.x + _zoomDelta))
        {
            _zoomIn = false;
            transform.localScale = _goalScale;
        }
    }

    private void ZoomOut()
    {
        if (!_zoomOut) return;
        transform.localScale = Vector3.Lerp(transform.localScale, _gameplayScale, _zoomSpeed * Time.deltaTime);
        if (transform.localScale.x >= (_gameplayScale.x + _zoomDelta))
        {
            _zoomOut = false;
            transform.localScale = _gameplayScale;
        }
    }

    private void FixedUpdate()
    {
        Zoom();
        ZoomOut();
    }
}
