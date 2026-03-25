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

    [SerializeField] ParticleSystem healParticles;
    [SerializeField] ParticleSystem hurtParticles;

    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        hurtParticles.Play();

        if (Health - damage <= 0)
        {
            Health = 0;
            OnDied.Invoke();
            return;
        }

        Health -= damage;
        Debug.Log($"Damage taken: {damage} ({Health})");
    }

    public void Heal(float value)
    {
        if (IsDead)
            return;

        healParticles.Play();

        Health = Mathf.Min(Health + value, MaxHealth);
        Debug.Log($"Healed: {value} ({Health})");
    }

    private void Awake()
    {
        Health = MaxHealth;
    }
}
