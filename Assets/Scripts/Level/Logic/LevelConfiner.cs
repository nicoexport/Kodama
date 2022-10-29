using System;
using Cinemachine;
using GameManagement;
using Player;
using UnityEngine;

namespace Level.Logic
{
   [RequireComponent(typeof(BoxCollider2D))]
   public class LevelConfiner : MonoBehaviour
   {
      [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;
      [SerializeField] private CharacterRuntimeSet _playerRuntimeSet;
      private Transform _playerTransform;
      private BoxCollider2D _collider;
      private Vector2 _center;

      private void Awake()
      {
         _collider = GetComponent<BoxCollider2D>();
         var position = transform.position;
         var offset = _collider.offset;
         _center = new Vector2(position.x + offset.x,
                               position.y + offset.y);
      }

      protected void Start()
      {
         SetCamCollider();
      }

      private void Update()
      {
        if (_playerTransform == null) GetPlayer();
        else
        {
            CheckBounds();
        }

      }
      
      private void GetPlayer()
      {
         if(_playerRuntimeSet.TryGetFirst(out Character player))
            _playerTransform = player.transform;
      }
      
      private void CheckBounds()
      {
         var position = _playerTransform.position;
         
         if (position.x < _center.x - _collider.size.x / 2)
            Test();
         if (position.x > _center.x + _collider.size.x / 2)
            Test();
         if (position.y > _center.y + _collider.size.y / 2)
            Test();
         if (position.y < _center.y - _collider.size.y / 2)
            Test();
      }

      private void Test()
      {
         if (_playerTransform.TryGetComponent(out PlayerHealth health))
         {
            health.Die();
         }
      }

      /*protected void OnTriggerExit2D(Collider2D other)
      {
         if (!other.CompareTag("Player"))
            return;
         if (!other.TryGetComponent(out PlayerHealth lifeCycleHandler)) return;
         lifeCycleHandler.Die();
      }*/

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
