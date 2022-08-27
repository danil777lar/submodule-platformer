using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private PlayerControll _playerControll;
    private Camera _mainCamera;

    public bool ForwardAnimationDirection { get; private set; }
    public PlayerAnimationType PlayingAnimation { get; private set; }


    private void Start()
    {
        _mainCamera = Camera.main;

        PlayingAnimation = PlayerAnimationType.Idle;
    }

    private void Update()
    {
        PlayingAnimation = _playerControll.Animation;

        if (_mainCamera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //ForwardAnimationDirection = _playerControll.IsForwardMoving;
            ForwardAnimationDirection = true;
        }
        else 
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //ForwardAnimationDirection = !_playerControll.IsForwardMoving;
            ForwardAnimationDirection = true;
        }
    }
}
