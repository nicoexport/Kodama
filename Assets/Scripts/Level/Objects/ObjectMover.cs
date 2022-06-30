using System.Collections;
using UnityEngine;

namespace Level.Objects
{
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField] Transform[] wayPoints;

        [SerializeField] float speed = 10f;

        [SerializeField] float pauseTime;

        int currentWayPointIndex;

        bool move;

        void Start()
        {
            currentWayPointIndex = 0;
            move = true;
        }

        void FixedUpdate()
        {
            MoveObject();
        }

        void MoveObject()
        {
            if (!move) return;
            transform.position =
                Vector2.MoveTowards(transform.position, wayPoints[currentWayPointIndex].position, speed / 100f);
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
}