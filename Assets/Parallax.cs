using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Parallax : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform _subject;
    private Vector2 _startPosition;
    private float _startZ;
    private Transform _transform;
    private Transform _cameraTransform;
    

    private void Awake()
    {
        _transform = transform;
        _camera = Camera.main;
        print(_camera.name);
        _cameraTransform = _camera.transform;
        _startPosition = transform.position;
        _startZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        if (!_cameraTransform) return;
        var distanceFromSubject = _transform.position.z - _subject.transform.position.z;
        var clippingPlane = _camera.transform.position.z +(distanceFromSubject>0? _camera.farClipPlane : _camera.nearClipPlane);
        var parallaxFactor = Mathf.Abs(distanceFromSubject / clippingPlane);
        var travel = (Vector2)_cameraTransform.position - _startPosition;
        var newPos = _startPosition + travel * parallaxFactor;
        _transform.position = new Vector3(newPos.x, newPos.y, _startZ);
    }
    
}
