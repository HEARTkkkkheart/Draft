using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum E_State
{
    Idle,
    walk,
    jump,
}


public class MoveController : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    private E_State _state;

    [Header("移动相关")]
    [SerializeField] private bool _isWalking;
    [SerializeField] private float _horizontalMoveDir;
    [SerializeField] private float _moveSpeed = 4f;

    [Header("跳跃相关")]
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isAllowJumpDown;
    [SerializeField] private bool _isPreJumpDown;
    [SerializeField] private bool _isJumpIncreaseSpeed;
    [SerializeField] private bool _isJumpUp;
    
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _jumpUpMaxSpeed = 4f;
    [SerializeField] private float _jumpDownMaxSpeed = -5.5f;
    [SerializeField] private float _jumpDeltaUpSpeed = 0.07f;
    [SerializeField] private float _jumpDeltaDownSpeed = 0.15f;

    [SerializeField] private float _allowJumpDownStamp = 0;
    [SerializeField] private float _allowJumpUpTime = 0.5f;

    public void OnTriggerFloor()
    {
        _isJumping = false;
        _isJumpUp = false;
        _isJumpIncreaseSpeed = false;
        _isPreJumpDown = false;
        ResetJumpSpeed();
    }


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void ResetJumpSpeed() => _jumpSpeed = 0;

    private void Update()
    {
        UpdateJump();
    }

    private void UpdateJump()
    {
        if (Mathf.Abs(0f - Input.GetAxis("Horizontal")) >= 0.0001)
        {
            _isWalking = true;
            _horizontalMoveDir = Input.GetAxis("Horizontal");
        }
        else
        {
            _isWalking = false;
        }

        if (Input.GetKeyDown(_jumpKey))
        {
            if (!_isJumping)
            {
                _isJumping = true;
                _isJumpUp = true;
                _isAllowJumpDown = false;
                _allowJumpDownStamp = Time.realtimeSinceStartup;
            }
        }
        if (Input.GetKey(_jumpKey))
        {
            if (_isJumping)
            {
                if (_isJumpUp)
                {
                    _isJumpIncreaseSpeed = true;
                }
            }
        }
        if (Input.GetKeyUp(_jumpKey))
        {
            if (_isJumping)
            {
                _isPreJumpDown = true;
            }
        }

        if (Time.realtimeSinceStartup - _allowJumpDownStamp > _allowJumpUpTime)
        {
            _isAllowJumpDown = true;
        }

        if (_isAllowJumpDown && _isPreJumpDown)
        {

            if (_isJumping)
            {
                _isJumpUp = false;
                _isJumpIncreaseSpeed = false;
            }
        }
    }

    private void CheckAndMaintenanceJumpSpeed()
    {
        if (!_isJumping) return;
        if (_isJumpUp)
        {
            if (_isJumpIncreaseSpeed)
            {
                _jumpSpeed += _jumpDeltaUpSpeed;
                if (_jumpSpeed > _jumpUpMaxSpeed)
                {
                    _jumpSpeed = _jumpDeltaUpSpeed;
                    _isJumpIncreaseSpeed = false;
                    _isJumpUp = false;
                    _isAllowJumpDown = true;
                }
            }else 
            {
                _jumpSpeed -= _jumpDeltaDownSpeed;
                if (_jumpSpeed < _jumpDownMaxSpeed)
                {
                    _jumpSpeed = _jumpDownMaxSpeed;
                }
                if (_jumpSpeed < 0)
                {
                    _isJumpUp = false;
                }
            }
        }
        else
        {
            _jumpSpeed -= _jumpDeltaDownSpeed;

            if (_jumpSpeed < _jumpDownMaxSpeed)
            {
                _jumpSpeed = _jumpDownMaxSpeed;
            }
        }
    }

    private void FixedUpdate()
    {
        var speedDir = _rigidbody2D.velocity;
        if (_isWalking)
        {
            speedDir.x = _horizontalMoveDir * _moveSpeed;
        }
        else
        {
            speedDir.x = 0;
        }
        if (_isJumping)
        {
            CheckAndMaintenanceJumpSpeed();
            speedDir.y = _jumpSpeed;
        }
        _rigidbody2D.velocity = speedDir;
    }

}
