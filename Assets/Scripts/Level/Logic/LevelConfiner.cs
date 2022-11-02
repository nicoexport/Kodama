using System;
using Cinemachine;
using GameManagement;
using Player;
using UnityEngine;

namespace Level.Logic
{
   public class LevelConfiner : MonoBehaviour
   {
      [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
      [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
      private Collider2D _playerCollider;
      private Collider2D _collider;
      private Rigidbody2D _rb;

      private void Awake()
      {
         TryGetComponent(out _collider);
         TryGetComponent(out _rb);
         var position = transform.position;
      }

      protected void Start()
      {
         SetCamCollider();
      }

      private void Update()
      {
        if (_playerCollider == null) 
           GetPlayerCollider();
        else
           CheckBounds();
      }

      private void GetPlayerCollider()
      {
         if (_playerRuntimeSet.TryGetFirst(out Character player))
            player.gameObject.TryGetComponent(out _playerCollider);
         
      }

      private void CheckBounds()
      {
         if(!_rb.OverlapPoint(_playerCollider.transform.position))
            Test();
      }

      private void Test()
      {
         if (_playerCollider.TryGetComponent(out PlayerHealth health))
         {
            health.Die();
         }
      }
      
      private void SetCamCollider()
      {
         var cmCam = _cinemachineRuntimeSet.GetItemAtIndex(0);
         if (cmCam == null)
            return;
         if (!cmCam.TryGetComponent(out CinemachineConfiner2D confiner)) 
            return;
         if (!TryGetComponent(out Collider2D col)) 
            return;
         confiner.m_BoundingShape2D = col;
      }
   }
}
