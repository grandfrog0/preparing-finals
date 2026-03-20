using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Entity _target;

    [SerializeField] float attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackCD;
    private float _currentAttackCD;

    private void FixedUpdate()
    {
        if (_currentAttackCD > 0)
            _currentAttackCD -= Time.fixedDeltaTime;

        Debug.Log((_target, _target ? Vector3.Distance(_target.transform.position, transform.position) <= attackRange : false));
        if (_target != null)
        {
            if (Vector3.Distance(_target.transform.position, transform.position) <= attackRange)
            {
                _agent.SetDestination(transform.position);
                if (_currentAttackCD <= 0)
                {
                    Attack();
                }
            }
            else
            {
                _agent.SetDestination(_target.transform.position);
            }
        }
    }

    private void Attack()
    {
        Debug.Log("Attack!");
        _target.TakeDamage(attackDamage);
        _currentAttackCD = attackCD;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log((_target, other.gameObject.name));
        if (_target != null)
            return;

        if (other.CompareTag("Player"))
        {
            _target = other.GetComponent<Entity>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_target != null && other.gameObject == _target.gameObject)
        {
            _target = null;
        }
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
}
