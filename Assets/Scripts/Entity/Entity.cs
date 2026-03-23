using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public UnityEvent OnDied;

    [SerializeField] float maxHealth;
    public float Health { get; private set; }
    public float MaxHealth => maxHealth;
    public float HealthPercent => Health / MaxHealth;
    public bool IsDead => Health <= 0;

    public void TakeDamage(float damage)
    {
        if (Health <= 0)
            return;

        if (Health - damage <= 0)
        {
            Health = 0;
            OnDied.Invoke();
            return;
        }

        Health -= damage;
        Debug.Log($"Damage taken: {damage} ({Health})");
    }

    private void Awake()
    {
        Health = MaxHealth;
    }
}
