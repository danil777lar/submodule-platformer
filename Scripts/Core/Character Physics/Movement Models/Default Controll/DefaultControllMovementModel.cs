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

        float horizontalSpeed = 0f;
        Vector2 resultPosition;
        if (_jumpTween == null)
        {
            horizontalSpeed = _speedPercent * (Input.GetKey(KeyCode.LeftShift) ? _config.FullRunSpeed : _config.FullWalkSpeed);
            if (_isGrounded)
            {
                _verticalSpeed = 0f;
            }
            else 
            {
                _verticalSpeed -= 9.8f * _config.GravityScale * deltaTime;
            }
            resultPosition = characterPosition + new Vector2(horizontalSpeed, _verticalSpeed) * deltaTime;
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
            if (LarjeUtility.CastCollider2D(_collider, (Vector2.right * _jumpSpeed).normalized, Mathf.Abs(_jumpSpeed * deltaTime))) 
            {
                _jumpSpeed = 0f;
            }
            _jumpSpeed = Mathf.Clamp(_jumpSpeed, -_config.FullRunSpeed, _config.FullRunSpeed);
            horizontalSpeed = _jumpSpeed;
            resultPosition = new Vector2(characterPosition.x + _jumpSpeed * deltaTime, _jumpValue);
        }

        if (LarjeUtility.CastCollider2D(_collider, (Vector2.right * horizontalSpeed).normalized, Mathf.Abs(horizontalSpeed * deltaTime)))
        {
            Vector2 raycastOrigin = _collider.transform.position;
            raycastOrigin.x += ((_collider.bounds.size.x / 2f) + 0.1f) * (horizontalSpeed < 0 ? -1 : 1);
            raycastOrigin.y += _collider.bounds.size.y + _config.StairHeight;
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down);
            float distanceToStair = Mathf.Abs(hit.point.y - _collider.transform.position.y);
            if (hit.point.y > _collider.transform.position.y && distanceToStair <= _config.StairHeight)
            {
                resultPosition.y = hit.point.y;
                _collider.gameObject.transform.position = resultPosition;
            }
            Debug.DrawLine(raycastOrigin, hit.point, Color.red); 
        }

        return resultPosition;
    }

    public SpriteAnimationInfo ComputeAnimation()
    {
        SpriteAnimationInfo anim = new SpriteAnimationInfo();
        anim.ForwardDirection = true;

        bool lookRight = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > _collider.transform.position.x;
        _collider.transform.localScale = new Vector3(lookRight ? 1 : -1, 1, 1);

        bool isNearToFloor = LarjeUtility.CastCollider2D(_collider, Vector2.down, _config.StairHeight);

        if ((_isGrounded && (_runLeftTween != null || _runRightTween != null)) || (!_isGrounded && isNearToFloor && _jumpTween == null))
        {
            if ((_runLeftTween != null && lookRight) || (_runRightTween != null && !lookRight)) 
            {
                anim.ForwardDirection = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.AnimationType = PlayerAnimationType.Run;
            }
            else
            {
                anim.AnimationType = PlayerAnimationType.Walk;
            }
        }
        else if (!_isGrounded)
        {
            if (_verticalSpeed > 0)
            {
                anim.AnimationType = PlayerAnimationType.Jump;
            }
            else if (_verticalSpeed < 0)
            {
                anim.AnimationType = PlayerAnimationType.Fall;
            }
        }
        else 
        {
            anim.AnimationType = PlayerAnimationType.Idle;            
        }

        return anim;
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
