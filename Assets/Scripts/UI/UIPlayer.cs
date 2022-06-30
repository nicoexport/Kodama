using UnityEngine;

namespace UI
{
    public class UIPlayer : MonoBehaviour, IUICharacter
    {
        static readonly int IsMoving = Animator.StringToHash("isMoving");
        Animator _animator;
        bool _facingRight = true;

        void Awake()
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
            if (!_facingRight) Flip();
        }

        void Flip()
        {
            _facingRight = !_facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}