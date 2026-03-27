using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EntityMovement
{
    private Rigidbody _rigidbody;
    private MyInputSystem _inputSystem;

    [Header("Jump")]
    [SerializeField] float jumpStrength = 10;
    [SerializeField] LayerMask floorMask;
    public float JumpStrength => jumpStrength * MovementSpeedMultiplier;
    public bool IsOnFloor => Physics.Raycast(new Ray(transform.position - Vector3.up * transform.localScale.y / 1.8f, Vector3.down), 1, floorMask, QueryTriggerInteraction.Ignore);
    
    [Header("Look")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float scrollSensitivity = 20;
    [SerializeField] float lookSpeed = 5;
    private Vector2 _camRot;

    [Header("Misc")]
    [SerializeField] PlayerInventory inventoryConfig;

    protected override void HandleMovement(Vector2 axis)
    {
        _isSprinting = _inputSystem.Player.Sprint.ReadValue<float>() > 0;
        Vector3 direction = new Vector3(axis.x * MovementSpeed, _rigidbody.linearVelocity.y, _axis.y * MovementSpeed);
        _rigidbody.linearVelocity = transform.rotation * direction;

        base.HandleMovement(axis);
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
        if (!_canMove)
            return;

        if (IsOnFloor)
        {
            _rigidbody.AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
            _animator.SetTrigger("OnJump");
        }
    }

    private void Update()
    {
        if (!_canMove)
            return;

        HandleMovement(_inputSystem.Player.Move.ReadValue<Vector2>());
        HandleLook();
    }
    private void FixedUpdate()
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position, lookSpeed * Time.fixedDeltaTime);
    }
    protected override void Awake()
    {
        _inputSystem = new MyInputSystem();
        _inputSystem.Player.Jump.performed += TryJump;

        _rigidbody = GetComponent<Rigidbody>();

        EntityShooter shooter = GetComponent<EntityShooter>();
        _inputSystem.Player.Shoot.performed += x => shooter.TryShoot();

        base.Awake();
    }
    private void Start()
    {
        InventoryContainer container = GetComponent<InventoryContainer>();
        container.InitInventory(inventoryConfig.Inventory);
    }
    private void OnEnable() => _inputSystem.Enable();
    private void OnDisable() => _inputSystem.Disable();

    public override void OnDied()
    {
        base.OnDied();
        HandleMovement(Vector2.zero);
    }
}
