using UnityEngine;

public interface IUICharacter
{
    void StartMoving(Transform goal);
    void StopMoving();
}