using System;
using UnityEngine;

namespace Scriptable.Channels
{
   public abstract class DuoEventChannelSO<T1,T2> : ScriptableObject
   {
      public Action<T1, T2> OnEventRaised;

      public void RaiseEvent(T1 item1, T2 item2)
      {
         OnEventRaised?.Invoke(item1, item2);
      }
   }
}
