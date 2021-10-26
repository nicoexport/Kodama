using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class ViewportHandler : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer targetSize;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetSize.bounds.size.x / targetSize.bounds.size.y;

        if (screenRatio >= targetRatio) cam.orthographicSize = targetSize.bounds.size.y / 2;
        else
        {
            float SizeDifference = targetRatio / screenRatio;
            cam.orthographicSize = targetSize.bounds.size.y / 2 * SizeDifference;
        }
    }
}