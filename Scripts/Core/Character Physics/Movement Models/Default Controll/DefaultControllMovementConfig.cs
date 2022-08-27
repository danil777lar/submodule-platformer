using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Scriptables/DefaultControllMovementConfig")]
public class DefaultControllMovementConfig : ScriptableObject
{
    [Header("Run")]
    public float FullRunSpeed;
    public float FullWalkSpeed;
    public float AccelerationDuration;
    public AnimationCurve AccelerationCurve;
    public float DecelerationDuration;
    public AnimationCurve DecelerationCurve;

    [Header("Jump")]
    public float JumpHeight;
    public float JumpDuration;
    public AnimationCurve JumpCurve;
    public float JumpHorizontalSpeedControll;
    public float GravityScale;
}
