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

      private void Awake()
      {
         _collider = GetComponent<BoxCollider2D>();
         var position = transform.position;
      }

      protected void Start()
      {
         SetCamCollider();
      }

      private void Update()
      {
        if (_playerCollider == null) GetPlayerCollider();
        else
        {
            CheckBounds();
        }

      }

      private void GetPlayerCollider()
      {
         if (_playerRuntimeSet.TryGetFirst(out Character player))
            player.TryGetComponent(out _collider);
      }

      private void CheckBounds()
      {
         Debug.Log(Physics2D.Distance(_collider, _playerCollider ));
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
