using System.Collections;
using UnityEngine;
    
public interface ISelectUI
{
    IEnumerator OnStart(SaveDataSo sessionData);
    IEnumerator OnEnd();
}
