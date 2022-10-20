using UnityEngine;

namespace Level.Objects
{
    public class ParentSetter : MonoBehaviour
    {
        [SerializeField] private bool onCollision;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (onCollision && col.collider.tag == "Player") col.collider.transform.SetParent(transform);
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (onCollision && col.collider.tag == "Player") col.collider.transform.SetParent(null);
        }
    }
}