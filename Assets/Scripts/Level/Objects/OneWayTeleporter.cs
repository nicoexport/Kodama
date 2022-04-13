using UnityEngine;

namespace Level.Objects
{
    public class OneWayTeleporter : MonoBehaviour
    {
        [SerializeField]
        private Transform exit;


        private void OnTriggerEnter2D(Collider2D entity)
        {
            if (entity.tag != "Player") return;
            Debug.Log("Player entered");
            entity.transform.position = exit.transform.position;
        }
    }
}
