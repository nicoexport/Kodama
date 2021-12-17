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
    private float pauseTime = 0f;

    private bool move;

    private void Start()
    {
        currentWayPointIndex = 0;
        move = true;
    }

    private void FixedUpdate()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        if (!move) return;
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWayPointIndex].position, speed / 100f);
        if (Vector2.Distance(transform.position, wayPoints[currentWayPointIndex].position) <= 0.1f)
        {
            currentWayPointIndex += 1;
            if (pauseTime != 0f) StartCoroutine(PauseMove(pauseTime));
        }
        if (currentWayPointIndex >= wayPoints.Length) currentWayPointIndex = 0;
    }

    public IEnumerator PauseMove(float delay)
    {
        move = false;
        yield return new WaitForSeconds(delay);
        move = true;
    }
}
