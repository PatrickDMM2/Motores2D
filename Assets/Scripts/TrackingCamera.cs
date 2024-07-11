using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Vector3 _offset = new Vector3(0f, 0f, -10f);
    public float _travelTime = 0.25f;
    public Vector3 _velocity = Vector3.zero;

    [SerializeField] private Transform target;

    void Update()
    {
            Vector3 targetPosition = target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _travelTime);
    }
}
