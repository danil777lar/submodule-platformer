using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePartAnimationPlayer : MonoBehaviour
{
    [SerializeField] private int _framesPerSecond;
    [SerializeField] private SpritePartAnimation _partAnimation;

    private int _currentFrame = 0;
    private IAnimationProcessor _characterAnimation;
    private SpriteRenderer _spriteRenderer;
    private SpriteAnimationInfo _currentAnim;


    private void Start()
    {
        _characterAnimation = GetComponentInParent<IAnimationProcessor>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentAnim = _characterAnimation.GetAnimationInfo();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateFrameCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void Update()
    {
        if (!_partAnimation) 
        {
            return;
        }

        if (_currentAnim.AnimationType != _characterAnimation.GetAnimationInfo().AnimationType) 
        {
            _currentAnim = _characterAnimation.GetAnimationInfo();
            _currentFrame = 0;
        }

        Sprite[] animationSprites = _partAnimation.GetAnimationByType(_currentAnim.AnimationType);
        if (_currentFrame > animationSprites.Length - 1 && (_characterAnimation.GetAnimationInfo().ForwardDirection || (_currentAnim.AnimationType == PlayerAnimationType.Jump || _currentAnim.AnimationType == PlayerAnimationType.Fall)))
        {
            if (_currentAnim.AnimationType == PlayerAnimationType.Jump || _currentAnim.AnimationType == PlayerAnimationType.Fall)
            {
                _currentFrame = animationSprites.Length - 1;
            }
            else 
            {
                _currentFrame = 0;
            }
        }
        else if (_currentFrame < 0 && !_characterAnimation.GetAnimationInfo().ForwardDirection) 
        {
            _currentFrame = animationSprites.Length - 1;
        }
        _currentFrame = Mathf.Clamp(_currentFrame, 0, animationSprites.Length - 1);
        _spriteRenderer.sprite = animationSprites[_currentFrame];
    }

    private IEnumerator UpdateFrameCoroutine() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f / _framesPerSecond);
            if (_characterAnimation.GetAnimationInfo().ForwardDirection || (_currentAnim.AnimationType == PlayerAnimationType.Jump || _currentAnim.AnimationType == PlayerAnimationType.Fall))
            {
                _currentFrame++;
            }
            else 
            {
                _currentFrame--;
            }
        }
    }

}
