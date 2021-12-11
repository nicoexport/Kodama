using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool reverse;


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<Rigidbody2D>() == null) return;
        Vector2 dir = Vector2.zero;
        if (!vertical) dir.x = point1.position.x - point2.position.x;
        else dir.y = point1.position.y - point2.position.y;
        if (reverse) dir = -dir;
        dir = dir.normalized;
        var rb = col.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * force * 10f, ForceMode2D.Force);
    }
}
