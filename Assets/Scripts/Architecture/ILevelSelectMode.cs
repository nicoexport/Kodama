using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface ILevelSelectMode
{
   LevelSelectModeState _state { get; }

   IEnumerator OnStart();
   IEnumerator OnEnd();
   
}

public enum LevelSelectModeState
{
   Starting,
   Started, 
   Ending,
   Ended
}