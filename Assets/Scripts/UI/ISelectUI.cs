using System.Collections;
using UnityEngine;
    
public interface ISelectUI
{
    IEnumerator OnStart(GameSessionDataSO sessionData);
    IEnumerator OnEnd();
}
