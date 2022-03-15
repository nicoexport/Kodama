using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 goalScale;
    [SerializeField]
    private float zoomSpeed = 1f;
    [SerializeField]
    private float zoomDelta = 0.001f;
    private bool zoom;

    private void OnEnable()
    {
        LevelManager.OnCompleteLevel += StartZoom;
    }

    private void OnDisable()
    {
        LevelManager.OnCompleteLevel -= StartZoom;
    }

    private void StartZoom()
    {
        zoom = true;
    }

    private void Zoom()
    {
        if (!zoom) return;
        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, zoomSpeed * Time.deltaTime);
        if (transform.localScale.x <= (goalScale.x + zoomDelta))
        {
            zoom = false;
            transform.localScale = goalScale;
        }
    }

    private void FixedUpdate()
    {
        Zoom();
    }
}
