using System.Collections;
using UnityEngine;
    
public interface ISelectUI
{
    IEnumerator OnStart(SessionData sessionData);
    IEnumerator OnEnd();
}
