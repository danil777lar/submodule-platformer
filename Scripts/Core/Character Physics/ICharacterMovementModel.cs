using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterMovementModel
{
    Vector2 ComputePosition(Vector2 characterPosition, float deltaTime);
    PlayerAnimationType ComputeAnimation();
}
