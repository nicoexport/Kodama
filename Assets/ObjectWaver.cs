using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWaver : MonoBehaviour
{
   [SerializeField] private bool _playOnAwake;
   [SerializeField] private float _speed;
   [SerializeField] private LeanTweenType _tweenType;
   [SerializeField] private Vector2 _targetOffset;
   private Vector2 _target;


   private void Awake()
   {
      _target = (Vector2) transform.position + _targetOffset;
      if (_playOnAwake)
         Wave();
   }

   private void Wave()
   {
      LeanTween.move(gameObject, _target, _speed).setEase(_tweenType).setLoopPingPong();
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.blue;
      var position = transform.position;
      Vector2 target2 = (Vector2) position + _targetOffset;
      Vector3 target = new Vector3(target2.x, target2.y, position.z);
      Gizmos.DrawLine(position, target);
      Gizmos.DrawCube(target, Vector3.one/2);
   }

 
}
