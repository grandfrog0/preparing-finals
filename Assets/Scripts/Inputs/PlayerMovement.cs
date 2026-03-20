using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private MyInputSystem _inputSystem;
    private Rigidbody _rigidbody;
    private Animator _animator;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float jumpStrength = 10;
    [SerializeField] LayerMask floorMask;
    private Vector2 _axis;
    public bool IsRunning => _axis.y > 0 && _inputSystem.Player.Sprint.ReadValue<float>() > 0;
    public float MovementSpeed => IsRunning ? runSpeed : walkSpeed;
    public bool IsOnFloor => Physics.Raycast(new Ray(transform.position - Vector3.up * transform.localScale.y / 1.8f, Vector3.down), 1, floorMask, QueryTriggerInteraction.Ignore);
    [Header("Look")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float scrollSensitivity = 20;
    [SerializeField] float lookSpeed = 5;
    private Vector2 _camRot;

    private void HandleMovement()
    {
        _axis = _inputSystem.Player.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(_axis.x * MovementSpeed, _rigidbody.linearVelocity.y, _axis.y * MovementSpeed);
        _rigidbody.linearVelocity = transform.rotation * direction;

        _animator.SetInteger("Axis", IsRunning ? 2 : _axis != Vector2.zero ? 1 : 0);
    }
    private void HandleLook()
    {
        Vector2 delta = _inputSystem.Player.Look.ReadValue<Vector2>() * scrollSensitivity * Time.deltaTime;

        _camRot.y += delta.x;
        _camRot.x -= delta.y;
        _camRot.x = Mathf.Clamp(_camRot.x, -80, 80);

        cameraTransform.rotation = Quaternion.Euler(_camRot.x, _camRot.y, 0);
        transform.rotation = Quaternion.Euler(0, _camRot.y, 0);
    }

    private void TryJump(InputAction.CallbackContext context)
    {
        if (IsOnFloor)
        {
            Debug.Log("OnJump");
            _rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            _animator.SetTrigger("OnJump");
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }
    private void FixedUpdate()
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position, lookSpeed * Time.fixedDeltaTime);
    }
    private void Awake()
    {
        _inputSystem = new MyInputSystem();
        _inputSystem.Player.Jump.performed += TryJump;

        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable() => _inputSystem.Enable();
    private void OnDisable() => _inputSystem.Disable();
}
