using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMove : MonoBehaviour
{
    public Transform _transform;

    public float _speed;
    public float _currentSpeed;

    public SpriteRenderer _spriteRenderer;

    void Start()
    {
        _currentSpeed = _speed;
        _transform.position += new Vector3(0, 0, 0);

    }

    void Update()
    {
        _transform = GetComponent<Transform>();
        _transform.position += new Vector3(_currentSpeed, 0, 0) * Time.deltaTime;

        if (_transform.position.x >= (6)) 
        {
            _currentSpeed = -_speed;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.flipX = true;
        }

        if (_transform.position.x <= (-6))
        {
            _currentSpeed = _speed;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.flipX = false;
        }
    }
}
