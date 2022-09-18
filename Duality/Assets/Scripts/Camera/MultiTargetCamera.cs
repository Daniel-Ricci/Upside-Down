using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> _targets;
    [SerializeField] private float _zoomLimiter;

    private float _smoothTime = 0.5f;
    private float _minZoom = 20.0f;
    private float _maxZoom = 7.0f;
    private Vector3 _velocity;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if(LevelManager.Instance.controlsEnabled)
        {
            if(_targets.Count == 0) return;

            var bounds = GetBounds();

            var newPos = bounds.center;
            newPos.z = transform.position.z;

            var newZoom = Mathf.Lerp(_maxZoom, _minZoom, Mathf.Max(bounds.size.x, bounds.size.y) / _zoomLimiter);

            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, _smoothTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
        }
    }

    private Bounds GetBounds()
    {
        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        foreach(Transform t in _targets)
        {
            bounds.Encapsulate(t.position);
        }
        return bounds;
    }
}
