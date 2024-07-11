using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovementFinal : MonoBehaviour
{
    private float _horizontal;
    private float _speed = 8f;
    private float _jumpingPower = 16f;
    private bool _isFacingRight = true;
    private bool _doubleJump;

    private bool _canDash = true;
    private bool _isDashing;
    private float _dashingPower = 6f;
    private float _dashingTime = 0.2f;
    private float _dashingCooldown = 1f;

    public float _sprintSpeed;
    private bool _isSprinting;

    public Animator _animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Update()
    {
        if (_isDashing)
        {
            _animator.SetBool("IsDashing", false);

            return;
        }

        _horizontal = Input.GetAxisRaw("Horizontal");

        _animator.SetFloat("Speed", Mathf.Abs(_horizontal));

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            _doubleJump = false;
            _animator.SetBool("IsJumping", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || _doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, _jumpingPower);

                _doubleJump = !_doubleJump;

                _animator.SetBool("IsJumping", true);
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.O) && _canDash)
        {
            StartCoroutine(Dash());
            _animator.SetBool("IsDashing", true);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }

        float speed;
        if (Input.GetKey(KeyCode.P))
        {
            speed = _sprintSpeed;
            _animator.SetBool("IsSprinting", true);
        }
        else  
        {
            speed = _speed;
            _animator.SetBool("IsSprinting", false);
        }

        rb.velocity = new Vector2(_horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        float origalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * _dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        tr.emitting = false;
        rb.gravityScale = origalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }
}
