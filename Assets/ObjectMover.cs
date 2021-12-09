using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    private Transform[] wayPoints;
    private int currentWayPointIndex = 0;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private bool movePlayer = true;


    private void Start()
    {
        currentWayPointIndex = 0;
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWayPointIndex].position, speed / 100f);
        if (Vector2.Distance(transform.position, wayPoints[currentWayPointIndex].position) <= 0.1f) currentWayPointIndex += 1;
        if (currentWayPointIndex >= wayPoints.Length) currentWayPointIndex = 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (movePlayer && col.collider.tag == "Player")
        {
            col.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            col.collider.transform.SetParent(null);
        }
    }
}
