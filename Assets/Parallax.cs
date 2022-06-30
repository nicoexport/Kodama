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
    [SerializeField] protected Transform _subject;
    protected Camera _camera;
    protected Vector2 _startPosition;
    protected float _startZ;
    protected Transform _transform;
    protected Transform CameraTransform;


    void Awake()
    {
        _transform = transform;
        _camera = Camera.main;
        print(_camera.name);
        CameraTransform = _camera.transform;
        var position = transform.position;
        _startPosition = position;
        _startZ = position.z;
    }

    protected virtual void FixedUpdate()
    {
        if (!CameraTransform) return;
        if (!_subject) return;
        var distanceFromSubject = _transform.position.z - _subject.transform.position.z;
        var clippingPlane = _camera.transform.position.z +(distanceFromSubject>0? _camera.farClipPlane : _camera.nearClipPlane);
        var parallaxFactor = Mathf.Abs(distanceFromSubject / clippingPlane);
        var travel = (Vector2)CameraTransform.position - _startPosition;
        var newPos = _startPosition + travel * parallaxFactor;
        _transform.position = new Vector3(newPos.x, newPos.y, _startZ);
    }
    
}
