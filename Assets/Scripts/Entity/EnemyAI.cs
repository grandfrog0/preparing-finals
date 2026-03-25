using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : EntityMovement
{
    private NavMeshAgent _agent;
    private Entity _target;

    [SerializeField] EnemyState _state;
    public EnemyState State { get => _state; private set => _state = value; }

    [Header("Attack")]
    [SerializeField] float seeRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackCD;
    private float _currentAttackCD;

    [Header("Patrol")]
    [SerializeField] LayerMask floorMask;
    [SerializeField] Vector2 patrolTime = new Vector2(2, 5);
    private float _patrolTimeLeft;
    private Vector3 _patrolPoint;
    private Vector3 _patrolStart;

    protected override void HandleMovement(Vector2 axis)
    {
        if (!_canMove)
            return;
        base.HandleMovement(axis);
    }

    private void FixedUpdate()
    {
        if (!_canMove)
            return;

        if (_currentAttackCD > 0)
            _currentAttackCD -= Time.fixedDeltaTime;

        if (_target != null)
        {
            if (Vector3.Distance(_target.transform.position, transform.position) <= attackRange)
            {
                SetState(EnemyState.Attack);
                if (_currentAttackCD <= 0)
                {
                    if (!_target.IsDead)
                    {
                        Attack();
                    }

                    if (_target.IsDead)
                    {
                        _target = null;
                        SetState(EnemyState.Patrol);
                    }
                }
            }
            else
            {
                _isSprinting = Vector3.Distance(_target.transform.position, transform.position) > seeRange / 2;
                SetState(_isSprinting ? EnemyState.Run : EnemyState.Walk);
                _agent.SetDestination(_target.transform.position);
            }
        }
        if (State == EnemyState.Patrol)
        {
            _isSprinting = false;

            if (Vector3.Distance(transform.position, _patrolPoint) <= 0.25f)
            {
                if (_axis != Vector2.zero) // on enter
                {
                    _agent.SetDestination(transform.position);
                    HandleMovement(Vector2.zero);

                    _patrolTimeLeft = Random.Range(patrolTime.x, patrolTime.y);
                }
            }

            if (_patrolTimeLeft > 0)
            {
                _patrolTimeLeft -= Time.fixedDeltaTime; // stay
            }

            if (_patrolTimeLeft <= 0 && _axis == Vector2.zero) // exit
            {
                Vector2 unitCircle = Random.onUnitCircle * seeRange;
                if (Physics.Raycast(_patrolStart + new Vector3(unitCircle.x, 10, unitCircle.y), Vector3.down, out RaycastHit hitInfo, 50, floorMask, QueryTriggerInteraction.Ignore))
                {
                    _patrolPoint = hitInfo.point;
                    _agent.SetDestination(_patrolPoint);
                    HandleMovement(Vector2.up);
                }
            }
        }
    }

    public override void OnDied()
    {
        SetState(EnemyState.Dead);
        base.OnDied();
    }

    public void SetState(EnemyState state)
    {
        _patrolTimeLeft = 0;

        switch (state)
        {
            case EnemyState.Idle or EnemyState.Dead:
                _agent.SetDestination(transform.position);
                HandleMovement(Vector2.zero);
                break;

            case EnemyState.Walk or EnemyState.Run:
                _agent.speed = MovementSpeed;
                HandleMovement(Vector2.up);
                break;

            case EnemyState.Patrol:
                _patrolStart = transform.position;
                _patrolTimeLeft = Random.Range(patrolTime.x, patrolTime.y);
                break;

            case EnemyState.Attack:
                _agent.SetDestination(transform.position);
                HandleMovement(Vector2.zero);
                break;
        }
        State = state;
    }

    private void Attack()
    {
        _target.TakeDamage(attackDamage);
        _currentAttackCD = attackCD;
        _animator.SetTrigger("OnArmKick");
    }

    private void OnTriggerStay(Collider other)
    {
        if (_target != null)
            return;

        if (other.CompareTag("Player") && other.TryGetComponent(out Entity entity) && !entity.IsDead)
        {
            _target = entity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_target != null && other.gameObject == _target.gameObject)
        {
            _target = null;
            SetState(EnemyState.Patrol);
        }
    }

    protected override void Awake()
    {
        GetComponent<SphereCollider>().radius = seeRange;
        _agent = GetComponent<NavMeshAgent>();
        base.Awake();

        _agent.speed = MovementSpeed;
        SetState(EnemyState.Patrol);
    }
}
