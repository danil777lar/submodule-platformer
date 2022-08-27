using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class PlayerControll : MonoBehaviour
{
    [SerializeField] private DefaultControllMovementConfig _movementConfig;
    [SerializeField] private LayerMask _raycastContactLayers;

    private ICharacterMovementModel _currentMovementModel;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _boxCollider;
    private TargetJoint2D _targetJoint;

    public PlayerAnimationType Animation { get; private set; }
    


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<CapsuleCollider2D>();
        _targetJoint = GetComponent<TargetJoint2D>();

        _currentMovementModel = new DefaultControllMovementModel(_movementConfig, _boxCollider);
    }

    private void FixedUpdate()
    {
        _targetJoint.target = _currentMovementModel.ComputePosition(transform.position, Time.fixedDeltaTime);
        Animation = _currentMovementModel.ComputeAnimation();
    }
}
