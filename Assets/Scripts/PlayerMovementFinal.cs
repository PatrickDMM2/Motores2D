using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovementFinal : MonoBehaviour
{
    private CustomInput _input;
    private float _horizontal;
    public float _speed = 8f;
    public float _jumpingPower = 16f;
    private bool _isFacingRight = true;
    private bool _doubleJump;

    private bool _isWallSliding;
    public float _wallSlidingSpeed = 2f;

    private InputAction _walljump;
    private bool _isWallJumping;
    private float _wallJumpingDirection;
    private float _wallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private float _wallJumpingDuration = 0.4f;
    private Vector2 _wallJumpingPower = new Vector2(8f, 16f);

    private InputAction _dash;
    private bool _canDash = true;
    private bool _isDashing;
    public float _dashingPower = 6f;
    public float _dashingTime = 0.2f;
    public float _dashingCooldown = 1f;

    private InputAction _shoot;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private InputAction _sprint;
    public float _sprintSpeed;
    private bool _isSprinting;

    public Text victoryText;

    public CoinManager _coins;

    public Animator _animator;

    AudioManager audioManager;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();


        _input = new();
        _dash = _input.Player.Dash;
        _dash.performed += OnDashPerformed;
        _shoot = _input.Player.Shoot;
        _shoot.performed += OnShootPerformed;
        //_sprint = _input.Player.Run;
        //_sprint.started += OnRunStarted;
        //_sprint.canceled += OnSprintCanceled;

    }

    private void OnEnable()
    {
        _input.Enable();
        //_input.Player.Jump.started += _ => Jump();
        _input.Player.Dash.started += _ => Dash();
        _shoot.Enable();
        //_runAction.Enable();

    }

    private void OnDisable()
    {
        _input.Disable();
        //_input.Player.Jump.started -= _ => Jump();
        _input.Player.Dash.started -= _ => Dash();
        _shoot.Disable();
        //_runAction.Disable();
    }



    void Update()
    {
        if (!PauseMenu.isPaused)
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

            if (Input.GetKeyDown(KeyCode.I))
            {
                audioManager.PlaySFX(audioManager.fireball);
                Shoot();
            }

            WallSlide();
            WallJump();

            if (!_isWallJumping)
            {
                Flip();
            }
        }
        
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

        if (!_isWallJumping)
        {
            rb.velocity = new Vector2(_horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && _horizontal != 0f)
        {
            _isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {

            _isWallSliding = false;

            _animator.SetBool("IsWallSliding", _isWallSliding);
        }
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        Shoot();
    }
    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        Dash();
    }
    private void OnWallJumpPerformed(InputAction.CallbackContext context)
    {
        WallJump();
    }


    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -transform.localScale.x;
            _wallJumpingCounter = _wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));

            _animator.SetBool("IsWallSliding", true);

        }
        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && _wallJumpingCounter > 0f)
        {
            _isWallJumping = true;
            rb.velocity = new Vector2(_wallJumpingDirection * _wallJumpingPower.x, _wallJumpingPower.y);
            _wallJumpingCounter = 0f;

            if (transform.localScale.x != _wallJumpingDirection)
            {
                _isFacingRight = !_isFacingRight;

                transform.Rotate(0f, 180f, 0f);
                //Vector2 localSclae = transform.localScale;
                //localSclae.x *= -1f;
                //transform.localScale = localSclae;
            }

            Invoke(nameof(StopWallJumping), _wallJumpingDuration);

            _animator.SetBool("IsWallSliding", false);
        }
    }

    private void StopWallJumping()
    {
        _isWallJumping = false;
    }

    private void Flip()
    {
        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        {
            _isFacingRight = !_isFacingRight;

            transform.Rotate(0f, 180f, 0f);
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        float origalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.right.x * _dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        tr.emitting = false;
        rb.gravityScale = origalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Victory")
        { 
            victoryText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        if (collision.gameObject.CompareTag("Victory"))
        {
            audioManager.PlaySFX(audioManager.victory);
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            audioManager.PlaySFX(audioManager.coin);
            Destroy(collision.gameObject);
            _coins.coinCount++;
        }
    }
}
