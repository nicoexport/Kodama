using UnityEngine;

namespace UI
{
    public class UIPlayer : MonoBehaviour, IUICharacter
    {
        private Animator _animator;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private bool _facingRight = true;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
    
        public void StartMoving(Transform goal)
        {
            _animator.SetBool(IsMoving, true);
            if (goal.position.x > transform.position.x && !_facingRight)
                Flip();
            if (goal.position.x < transform.position.x && _facingRight)
                Flip();
        }

        public void StopMoving()
        {
            _animator.SetBool(IsMoving, false);
            if(!_facingRight) Flip();
        }

        private void Flip()
        {
            _facingRight = !_facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}