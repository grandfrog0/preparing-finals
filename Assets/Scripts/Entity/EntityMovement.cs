using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class EntityMovement : MonoBehaviour
{
    protected Animator _animator;

    [Header("Movement")]
    [SerializeField] protected float walkSpeed = 5;
    [SerializeField] protected float runSpeed = 10;
    protected bool _canMove = true;
    protected Vector2 _axis;
    protected bool _isSprinting;
    public bool IsRunning => _axis.y > 0 && _isSprinting;
    public float MovementSpeed => MovementSpeedMultiplier * (IsRunning ? runSpeed : walkSpeed);
    public float MovementSpeedMultiplier { get; set; } = 1;

    protected virtual void HandleMovement(Vector2 axis)
    {
        if (!_canMove)
            return;

        _axis = axis;
        _animator.SetInteger("Axis", IsRunning ? 2 : _axis != Vector2.zero ? 1 : 0);
    }
    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public virtual void OnDied()
    {
        _canMove = false;
        _animator.SetBool("IsDead", true);
    }
}
