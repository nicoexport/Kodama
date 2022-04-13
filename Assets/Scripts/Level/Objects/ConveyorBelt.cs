using Player;
using UnityEngine;

namespace Level.Objects
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField]
        private Transform point1;
        [SerializeField]
        private Transform point2;
        [SerializeField]
        private float force;
        [SerializeField]
        private bool vertical = false;
        [SerializeField]
        private bool down;
        [SerializeField]
        private bool reverse;


        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag != "Player") return;
            if (vertical && down)
            {
                Vector2 dir = Vector2.zero;
                if (!vertical) dir.x = point1.position.x - point2.position.x;
                else dir.y = point1.position.y - point2.position.y;
                if (reverse) dir = -dir;
                dir = dir.normalized;
                var rb = col.collider.GetComponent<Rigidbody2D>();
                rb.AddForce(dir * force * 1.5f, ForceMode2D.Impulse);
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (vertical && col.collider.tag == "Player")
            {
                var character = col.collider.GetComponent<Character>();
                if (character.GetState() != character.wallsliding) return;
            }
            if (col.collider.GetComponent<Rigidbody2D>() == null) return;
            Vector2 dir = Vector2.zero;
            if (!vertical) dir.x = point1.position.x - point2.position.x;
            else dir.y = point1.position.y - point2.position.y;
            if (reverse) dir = -dir;
            dir = dir.normalized;
            var rb = col.collider.GetComponent<Rigidbody2D>();
            rb.AddForce(dir * force * 10f, ForceMode2D.Force);
        }
    }
}
