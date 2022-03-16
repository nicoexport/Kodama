using UnityEngine;

public class UIPlayer : MonoBehaviour, IUICharacter
{
    private Animator _animator;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void StartMoving()
    {
        _animator.SetBool(IsMoving, true);
    }

    public void StopMoving()
    {
        _animator.SetBool(IsMoving, false);
    }
}