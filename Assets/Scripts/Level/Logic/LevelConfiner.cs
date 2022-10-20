using Cinemachine;
using GameManagement;
using Player;
using UnityEngine;

namespace Level.Logic
{
   public class LevelConfiner : MonoBehaviour
   {
      [SerializeField] private GameObjectRuntimeSet _cinemachineRuntimeSet;

      protected void Start()
      {
         SetCamCollider();
      }
      
      protected void OnTriggerExit2D(Collider2D other)
      {
         if (!other.CompareTag("Player"))
            return;
         if (!other.TryGetComponent(out PlayerHealth lifeCycleHandler)) return;
         lifeCycleHandler.Die();
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
