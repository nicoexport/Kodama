using Player;
using UnityEngine;

namespace Level.Objects
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] Transform point1;

        [SerializeField] Transform point2;

        [SerializeField] float force;

        [SerializeField] bool vertical;

        [SerializeField] bool down;

        [SerializeField] bool reverse;


        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.tag != "Player") return;
            if (vertical && down)
            {
                var dir = Vector2.zero;
                if (!vertical) dir.x = point1.position.x - point2.position.x;
                else dir.y = point1.position.y - point2.position.y;
                if (reverse) dir = -dir;
                dir = dir.normalized;
                var rb = col.collider.GetComponent<Rigidbody2D>();
                rb.AddForce(dir * force * 1.5f, ForceMode2D.Impulse);
            }
        }

        void OnCollisionStay2D(Collision2D col)
        {
            if (vertical && col.collider.tag == "Player")
            {
                var character = col.collider.GetComponent<Character>();
                if (character.GetState() != character.wallsliding) return;
            }

            if (col.collider.GetComponent<Rigidbody2D>() == null) return;
            var dir = Vector2.zero;
            if (!vertical) dir.x = point1.position.x - point2.position.x;
            else dir.y = point1.position.y - point2.position.y;
            if (reverse) dir = -dir;
            dir = dir.normalized;
            var rb = col.collider.GetComponent<Rigidbody2D>();
            rb.AddForce(dir * force * 10f, ForceMode2D.Force);
        }
    }
}