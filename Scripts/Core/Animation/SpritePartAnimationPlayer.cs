using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePartAnimationPlayer : MonoBehaviour
{
    [SerializeField] private int _framesPerSecond;
    [SerializeField] private SpritePartAnimation _partAnimation;

    private int _currentFrame = 0;
    private CharacterAnimation _characterAnimation;
    private SpriteRenderer _spriteRenderer;
    private PlayerAnimationType _currentAnim;


    private void Start()
    {
        _characterAnimation = GetComponentInParent<CharacterAnimation>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentAnim = _characterAnimation.PlayingAnimation;
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

        if (_currentAnim != _characterAnimation.PlayingAnimation) 
        {
            _currentAnim = _characterAnimation.PlayingAnimation;
            _currentFrame = 0;
        }

        Sprite[] animationSprites = _partAnimation.GetAnimationByType(_currentAnim);
        if (_currentFrame > animationSprites.Length - 1 && (_characterAnimation.ForwardAnimationDirection || (_currentAnim == PlayerAnimationType.Jump || _currentAnim == PlayerAnimationType.Fall)))
        {
            if (_currentAnim == PlayerAnimationType.Jump || _currentAnim == PlayerAnimationType.Fall)
            {
                _currentFrame = animationSprites.Length - 1;
            }
            else 
            {
                _currentFrame = 0;
            }
        }
        else if (_currentFrame < 0 && !_characterAnimation.ForwardAnimationDirection) 
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
            if (_characterAnimation.ForwardAnimationDirection || (_currentAnim == PlayerAnimationType.Jump || _currentAnim == PlayerAnimationType.Fall))
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
