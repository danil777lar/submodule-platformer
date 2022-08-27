using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Larje.Core.Services;
using Larje.Core.Utility;

public class DefaultControllMovementModel : ICharacterMovementModel
{
    private bool _isGrounded;
    private bool _isCeiled;
    private float _jumpValue;
    private float _lastJumpValue;
    private float _speedPercent;
    private float _verticalSpeed;
    private float _jumpSpeed;
    private Vector2 _characterPosition;

    private Tween _jumpTween;
    private Tween _stopTween;
    private Tween _runLeftTween;
    private Tween _runRightTween;
    private Collider2D _collider;
    private KeyboardInputService _inputService;
    private DefaultControllMovementConfig _config;


    public DefaultControllMovementModel(DefaultControllMovementConfig config, Collider2D collider)
    {
        _config = config;
        _collider = collider;
        _inputService = ServiceLocator.Default.GetService<KeyboardInputService>();

        _inputService.OnKeyDown += OnKeyDown;
        _inputService.OnKeyUp += OnKeyUp;
    }


    public Vector2 ComputePosition(Vector2 characterPosition, float deltaTime)
    {
        _isGrounded = LarjeUtility.CastCollider2D(_collider, Vector2.down, 0.1f);
        _isCeiled = LarjeUtility.CastCollider2D(_collider, Vector2.up, 0.1f);
        _characterPosition = characterPosition;


        if (_jumpTween == null)
        {
            float speed = _speedPercent * (Input.GetKey(KeyCode.LeftShift) ? _config.FullRunSpeed : _config.FullWalkSpeed);
            if (_isGrounded)
            {
                _verticalSpeed = 0f;
            }
            else 
            {
                _verticalSpeed -= 9.8f * _config.GravityScale * deltaTime;
            }
            return characterPosition + new Vector2(speed, _verticalSpeed) * deltaTime;
        }
        else
        {
            _verticalSpeed = (_jumpValue - _lastJumpValue) / deltaTime;
            _lastJumpValue = _jumpValue;
            if (Input.GetKey(KeyCode.A)) 
            {
                _jumpSpeed -= _config.JumpHorizontalSpeedControll * deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _jumpSpeed += _config.JumpHorizontalSpeedControll * deltaTime;
            }
            _jumpSpeed = Mathf.Clamp(_jumpSpeed, -_config.FullRunSpeed, _config.FullRunSpeed);
            return new Vector2(characterPosition.x + _jumpSpeed * deltaTime, _jumpValue);
        }
    }

    public PlayerAnimationType ComputeAnimation()
    {
        if (_isGrounded && (_runLeftTween != null || _runRightTween != null))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return PlayerAnimationType.Run;
            }
            else 
            {
                return PlayerAnimationType.Walk; 
            }
        }

        if (!_isGrounded) 
        {
            return PlayerAnimationType.Jump;
        }

        return PlayerAnimationType.Idle;
    }

    private void OnKeyDown(KeyCode key) 
    {
        if (key == KeyCode.Space) 
        {
            if (_isGrounded) 
            {
                _jumpSpeed = _speedPercent * (Input.GetKey(KeyCode.LeftShift) ? _config.FullRunSpeed : _config.FullWalkSpeed);
                float startValue = _characterPosition.y;
                _jumpTween = DOTween.To(() => 0f, (v) =>
                {
                    if ((v >= 0.5f && _isGrounded) || _isCeiled)
                    {
                        _jumpValue = _characterPosition.y;
                        _verticalSpeed = 0f;
                        _jumpTween.Kill();
                        _jumpTween = null;
                    }
                    else
                    {
                        _jumpValue = startValue + (_config.JumpHeight * _config.JumpCurve.Evaluate(v)); 
                    }
                }, 1f, _config.JumpDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => _jumpTween = null);
            }
        }
        if (key == KeyCode.A)
        {
            float startValue = _speedPercent;
            _stopTween?.Kill();
            _runLeftTween?.Kill();
            _runRightTween?.Kill();
            _runRightTween = null;
            _runLeftTween = DOTween.To(() => 0f, (v) => 
            {
                _speedPercent = Mathf.Lerp(startValue, -1f, _config.AccelerationCurve.Evaluate(v));
            }, 1f, _config.AccelerationDuration);
        }
        if (key == KeyCode.D) 
        {
            float startValue = _speedPercent;
            _stopTween?.Kill();
            _runRightTween?.Kill();
            _runLeftTween?.Kill();
            _runLeftTween = null;
            _runRightTween = DOTween.To(() => 0f, (v) =>
            {
                _speedPercent = Mathf.Lerp(startValue, 1f, _config.AccelerationCurve.Evaluate(v));
            }, 1f, _config.AccelerationDuration);
        }
    }

    private void OnKeyUp(KeyCode key)
    {
        if ((key == KeyCode.A && _runLeftTween != null) || (key == KeyCode.D && _runRightTween != null))
        {
            float startValue = _speedPercent;
            _runLeftTween?.Kill();
            _runRightTween?.Kill();
            _stopTween?.Kill();
            _runLeftTween = null;
            _runRightTween = null;
            _stopTween = DOTween.To(() => 0f, (v) =>
            {
                _speedPercent = Mathf.Lerp(startValue, 0f, _config.DecelerationCurve.Evaluate(v));
            }, 1f, _config.DecelerationDuration);
        }
    }
}
