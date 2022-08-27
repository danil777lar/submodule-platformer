using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/SpritePartAnimation")]
public class SpritePartAnimation : ScriptableObject
{
    [SerializeField] private List<Sprite> IdleAnimation;
    [SerializeField] private List<Sprite> WalkAnimation;
    [SerializeField] private List<Sprite> RunAnimation;
    [SerializeField] private List<Sprite> JumpAnimation;
    [SerializeField] private List<Sprite> FallAnimation;


    public Sprite[] GetAnimationByType(PlayerAnimationType animType) 
    {
        switch (animType) 
        {
            case PlayerAnimationType.Idle: return IdleAnimation.ToArray();
            case PlayerAnimationType.Walk: return WalkAnimation.ToArray();
            case PlayerAnimationType.Run: return RunAnimation.ToArray();
            case PlayerAnimationType.Jump: return JumpAnimation.ToArray();
            case PlayerAnimationType.Fall: return FallAnimation.ToArray();
            default: return null;
        }
    }
}

public enum PlayerAnimationType
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall
}