using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Vector3 _offset = new Vector3(0f, 0f, -10f);
    public float _travelTime = 0.25f;
    public Vector3 _velocity = Vector3.zero;
    public BoxCollider2D _boxCollider;

    [SerializeField] private Transform target;

    void Update()
    {
        if (target.transform.position >= _boxCollider.size)
        {
            Vector3 targetPosition = target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _travelTime);
        }
    }

    //Transform _target;
    //Vector3 _velocity = Vector3.zero;

    //[Range(0, 1)]
    //public float _smoothTime;

    //public Vector3 _positionOffset;

    //[Header("Axis Limit")]
    //public Vector2 _xLimit;
    //public Vector2 _yLimit;


    //private void Awake()
    //{
    //    _target = GameObject.FindGameObjectWithTag("Player").transform;
    //}

    //private void LateUpdate()
    //{
    //    Vector3 targetPosition = _target.position + _positionOffset;
    //    targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, _xLimit.x, _xLimit.y), Mathf.Clamp(targetPosition.y, _yLimit.x, _yLimit.y), -10);
    //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    //}
}
