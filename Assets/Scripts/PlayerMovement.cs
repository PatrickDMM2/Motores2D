using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CustomInput _input = null;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    public float _moveSpeed = 4f;
    private Animator _animator = null;

    private void Awake()
    {
        _input = new CustomInput();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += OnMovementPerformed;
        _input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= OnMovementPerformed;
        _input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveVector * _moveSpeed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext callbackContext)
    { 
        _moveVector = callbackContext.ReadValue<Vector2>();

        if(_moveVector.x > 0)
        {
            transform.localScale = new Vector3(3.4311f, 3.4311f, 3.4311f);
        }
        else
        {
            transform.localScale = new Vector3(-3.4311f, 3.4311f, 3.4311f);
        }

        if(_moveVector.y > 0)
        {
            _animator.SetBool("Jump", true);
        }

        _animator.SetBool("Jump", true);
        _animator.SetBool("Idle", true);
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _moveVector = Vector2.zero;
        _animator.SetBool("Idle", false);
        _animator.SetBool("Jump", false);
    }
}
