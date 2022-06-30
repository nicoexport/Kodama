using UnityEngine;

namespace Level.Objects
{
    public class ParentSetter : MonoBehaviour
    {
        [SerializeField] bool onCollision;

        void OnCollisionEnter2D(Collision2D col)
        {
            if (onCollision && col.collider.tag == "Player") col.collider.transform.SetParent(transform);
        }

        void OnCollisionExit2D(Collision2D col)
        {
            if (onCollision && col.collider.tag == "Player") col.collider.transform.SetParent(null);
        }
    }
}